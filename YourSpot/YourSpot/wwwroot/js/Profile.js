function submitPasswordChangeForm() {
    var form = $('#changePasswordForm');
    var messageDiv = $('#passwordMessage');


    $.ajax({
        url: form.attr('action'),
        type: 'POST',
        data: form.serialize(),
        success: function (response) {
           
            if (response.includes("Password successfully changed")) {
                messageDiv.html(response);
                messageDiv.css("color", "green");
                setTimeout(function () {
                    location.reload();  
                }, 2000);
            } else {
               
                messageDiv.html(response);
                messageDiv.css("color", "red");  
            }
        },
        error: function (xhr, status, error) {
         
            messageDiv.html("An error occurred. Please try again.");
            messageDiv.css("color", "red");  
        }
    });

   
    return false;
}
