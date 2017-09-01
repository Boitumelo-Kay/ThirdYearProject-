function Login() {
    //var email = "\'"+$("#email").val()+"'/";
    //var password = "\'" + $("#password").val() + "'/";;

    var email = $("#email").val();
    var password = $("#password").val();

    var loginRequest = "{ \"U_Email\": \"test@gmail.com\", \"U_Password\": \"1234\"}"

    var frmData = $("#LoginForm").serialize();
    

    $.ajax({
        dataType: 'JSON',
        data: {
            'email': String(email),
            'password': String(password)
        },
        type: 'POST',
        url: "http://localhost:13689/api/Web/PostWebLogin",
        success: function (result) {
            if (result) {
                if(result.includes("chef"))
                {
                    /*alert("Welcome");*/
                    $("#logModal").modal("hide");
                    window.location = "http://localhost:13689/Chef/ChefMeals/?email="+email;
                }
                else if(result.includes("cust"))
                {
                    
                    sessionStorage.setItem('LogIn', email);
                    $("#logModal").modal("hide");
                }
                else
                {
                    alert("Your Login Details were incorrect. Please try again")
                }

            }
           
           
           
       }

    })

}
/*
function EditProduct(productID, name, price, description, category) {
    $.ajax({
        dataType: 'JSON',
        url: "/Report/EditProduct?ID=" + productID + "&name=" + $(name).val() + "&price=" + $(price).val() + "&description=" + $(description).val() + "&category=" + $(category).val(),
        success: function (result) {
            $('#nname_' + productID).html(result["Name"]);
            $('#pprice_' + productID).html("R " + result["Price"]);
            $('#ddescription_' + productID).html(result["Description"]);
            $('#ccategory_' + productID).html(result["Category"]).add('selected');
        }
    });
*/