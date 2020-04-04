using CarRentalApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace CarRentalApp.Controllers
{
    public class PaygateController : ApiController
    {
        private string paygateId = "10011072130";
        private string paygateKey = "secret";
        ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        public async Task<IHttpActionResult> GetRequest(Guid id)
        {
            var client = new HttpClient();
            var request = new Dictionary<string, string>();
            var model = db.Bookings.Where(m => m.Id == id).Include(m => m.RentalItems).Include(m => m.ApplicationUser).FirstOrDefault();

            var totCharge = model.DropOffDate.Subtract(model.PickUpDate).TotalDays * (double)model.RentalItems.Rate;
            double totInsure = 0, totExtra = 0;

            foreach (var item in db.InsuranceBookings.Where(m => m.BookingId == id).ToList())
            {
                totInsure += db.Insurance.FirstOrDefault(m => m.Id == item.InsuranceId).Price;
            }
            foreach (var item in db.ExtrasBooking.Where(m => m.BookingId == id).ToList())
            {
                totExtra += db.Extras.FirstOrDefault(m => m.Id == item.ExtraId).Price;
            }
            var amount = (totCharge + totInsure + totExtra) * 100;

            request.Add("PAYGATE_ID", paygateId);
            request.Add("REFERENCE", $"{model.Reference}");
            request.Add("AMOUNT", $"{amount}");
            request.Add("CURRENCY", "ZAR");
            request.Add("RETURN_URL", $"{Request.RequestUri.Scheme}://{Request.RequestUri.Authority}/api/paygate");
            request.Add("TRANSACTION_DATE", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            request.Add("LOCALE", "en-za");
            request.Add("COUNTRY", "ZAF");
            request.Add("EMAIL", $"{model.ApplicationUser.Email}");

            request.Add("CHECKSUM", GetMd5Hash(request, paygateKey));

            var requestString = ToUrlEncodedString(request);

            var content = new StringContent(requestString, Encoding.UTF8, "application/x-www-form-urlencoded");

            var response = await client.PostAsync("https://secure.paygate.co.za/PayWeb3/initiate.trans", content);

            var responseContent = await response.Content.ReadAsStringAsync();

            var results = ToDictionary(responseContent);

            if (results.Keys.Contains("ERROR"))
            {
                return InternalServerError(new Exception(results["ERROR"]));
            }

            if (!VerifyMd5Hash(results, paygateKey, results["CHECKSUM"]))
            {
                return InternalServerError(new Exception("MD5 verification failed"));
            }

            AddTransaction(request, results["PAY_REQUEST_ID"], id);

            return Ok(results);
        }

        [HttpPost]
        public async Task<IHttpActionResult> PostPayment()
        {
            var responseContent = await Request.Content.ReadAsStringAsync();

            var results = ToDictionary(responseContent);

            var transaction = GetTransaction(results["PAY_REQUEST_ID"]);

            if (transaction == null)
            {
                return Redirect($"{Request.RequestUri.Scheme}://{Request.RequestUri.Authority}/finished.html?id=-2");
            }

            // Reorder attributes for MD5 check
            var validationSet = new Dictionary<string, string>();
            validationSet.Add("PAYGATE_ID", paygateId);
            validationSet.Add("PAY_REQUEST_ID", results["PAY_REQUEST_ID"]);
            validationSet.Add("TRANSACTION_STATUS", results["TRANSACTION_STATUS"]);
            validationSet.Add("REFERENCE", transaction.Reference);

            if (!VerifyMd5Hash(validationSet, paygateKey, results["CHECKSUM"]))
            {
                return null;
                //return Redirect($"{Request.RequestUri.Scheme}://{Request.RequestUri.Authority}/finished.html?id=-1");
            }
            string reff = transaction.Reference.Replace("#", "");
            if (results["TRANSACTION_STATUS"] == "1")
            {
                var id = db.Transaction.Where(m => m.PayRequestId == transaction.PayRequestId).FirstOrDefault()?.BookingId;
                return Redirect($"{Request.RequestUri.Scheme}://{Request.RequestUri.Authority}/results?id={id}");
            }
            else
            {
                return Redirect($"{Request.RequestUri.Scheme}://{Request.RequestUri.Authority}/");
            }

        }

        #region Database

        private void AddTransaction(Dictionary<string, string> request, string payRequestId, Guid id)
        {
            var db = new ApplicationDbContext();

            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                TransactionDate = DateTime.Now,
                PayRequestId = payRequestId,
                Reference = request["REFERENCE"],
                Amount = int.Parse(request["AMOUNT"]),
                EmailAddress = request["EMAIL"],
                BookingId = id
            };

            db.Transaction.Add(transaction);

            db.SaveChanges();
        }

        private Transaction GetTransaction(string payRequestId)
        {
            var db = new ApplicationDbContext();

            var transaction = db.Transaction.FirstOrDefault(record => record.PayRequestId == payRequestId);

            return transaction;
        }

        #endregion Database

        #region Encoding/Decoding

        private string ToUrlEncodedString(Dictionary<string, string> request)
        {
            var builder = new StringBuilder();

            foreach (string key in request.Keys)
            {
                builder.Append("&");
                builder.Append(key);
                builder.Append("=");
                builder.Append(HttpUtility.UrlEncode(request[key]));
            }

            var result = builder.ToString().TrimStart('&');

            return result;
        }

        private Dictionary<string, string> ToDictionary(string response)
        {
            var result = new Dictionary<string, string>();

            var valuePairs = response.Split('&');
            foreach (string valuePair in valuePairs)
            {
                var values = valuePair.Split('=');
                result.Add(values[0], HttpUtility.UrlDecode(values[1]));
            }

            return result;
        }

        #endregion Encoding/Decoding

        #region MD5 Hashing

        // Adapted from
        // https://msdn.microsoft.com/en-us/library/system.security.cryptography.md5(v=vs.110).aspx

        private string GetMd5Hash(Dictionary<string, string> data, string encryptionKey)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                var input = new StringBuilder();
                foreach (string value in data.Values)
                {
                    input.Append(value);
                }

                input.Append(encryptionKey);

                byte[] hash = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input.ToString()));

                StringBuilder sBuilder = new StringBuilder();

                for (int i = 0; i < hash.Length; i++)
                {
                    sBuilder.Append(hash[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }

        private bool VerifyMd5Hash(Dictionary<string, string> data, string encryptionKey, string hash)
        {
            var hashDict = new Dictionary<string, string>();

            foreach (string key in data.Keys)
            {
                if (key != "CHECKSUM")
                {
                    hashDict.Add(key, data[key]);
                }
            }

            string hashOfInput = GetMd5Hash(hashDict, encryptionKey);

            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion MD5 Hashing
    }
}
