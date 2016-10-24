(function ($, W, D) {
    var JQUERY4U = {};

    JQUERY4U.UTIL =
    {
        setupFormValidation: function () {
            //form validation rules
            $("#registration-form").validate({
                rules: {
                    FirstName: "required",
                    LastName: "required",
                    Country: "required",
                    State: {
                        required: true
                    },
                    AgentorMember:"required",
                    City: {
                        required:true,
                    },
                    Email: {
                        required: true,
                        email: true
                    },
                    Password:
                        {
                            required: true,
                            minlength: 8
                        },
                    ConfirmPassword:
                        {
                            required: true,
                            equalTo: "#Password_Id"
                        },
                    Pincode: {
                        required: true,
                        minlength: 6,
                        maxlength: 6,
                        digits: true
                    },
                    Mobile: {
                        required: true,
                        minlength: 10,
                        maxlength: 10,
                        digits: true
                    }
                },
                messages: {
                    FirstName: "Please enter your firstname",
                    LastName: "Please enter your lastname",
                    Country: "Please Select your Country",
                    AgentorMember:"Please Select your role",
                    State:{
                        required: "Please Select your State",
                        notEqual:"There is Some Error"
                    },
                    City: "Please Select your City",
                    Password: {
                        required: "Please Enter Password",
                        minlength: "Password contains minimum 8 charectors"
                    },
                    ConfirmPassword: {
                        required: "Please Enter Confirm Password",
                        equalTo: "Password and Confirm Password must be same"
                    },
                    Mobile: {
                        required: "Please provide a PhoneNumber",
                        minlength: "PhoneNumber Must be 10 Numbers",
                        maxlength: "PhoneNumber Must be 10 Numbers",
                        digits: "PhoneNumber contains only Numbers"
                    },
                    Pincode: {
                        required: "Please provide a PinCode",
                        minlength: "PinCode Must be 6 Numbers",
                        maxlength: "PinCode Must be 6 Numbers",
                        digits: "PinCode contains only Numbers"
                    },
                    Email: {
                        required: "Please Enter Email address",
                        email: "InValid Email Address"
                    },
                },
                submitHandler: function (form) {
                    form.submit();
                }
            });
        }
    }
    $(D).ready(function ($) {
        JQUERY4U.UTIL.setupFormValidation();
    });
})(jQuery, window, document);



//Validations for Login Form.
(function ($, W, D) {
    var JQUERY4U = {};

    JQUERY4U.UTIL =
    {
        setupFormValidation: function () {
            //form validation rules
            $("#login-form").validate({
                rules: {
                    Email:{ 
                        required: true,
                        email:true
                    },
                    Password: "required",
                },
                messages: {
                    Email:
                        {
                            required:"Please enter your Email",
                            email:"Please Enter Valid Email"
                        },
                    Password: "Please enter your Password"
                },
                submitHandler: function (form) {
                    form.submit();
                }
            });
        }   
    }
    $(D).ready(function ($) {
        JQUERY4U.UTIL.setupFormValidation();
    });
})(jQuery, window, document);