function ShowImagePreview(imageUploader, imagePreview) {
    if (imageUploader.files && imageUploader.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $(imagePreview).attr('src', e.target.result);
        }
        reader.readAsDataURL(imageUploader.files[0]);
    }
}