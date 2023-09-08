const wrapper = document.querySelector(".wrapper");
const iconClose = document.querySelector(".icon-close");
const overlay = document.querySelector(".overlay");

const loginLink = document.querySelector(".login-link");
const registerLink = document.querySelector(".register-link");

registerLink.addEventListener("click", () => {
  wrapper.classList.add("active");
});

loginLink.addEventListener("click", () => {
  wrapper.classList.remove("active");
});

const openPopup = function () {
  wrapper.classList.add("active-popup");
  overlay.style.display = "block";
};
const closePopup = function () {
  wrapper.classList.remove("active-popup");
  overlay.style.display = "none";
};



function isValidEmail(email) {
  // Use a regular expression to check if the email format is valid
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  return emailRegex.test(email);
}


//Login Form
document
  .getElementById("loginForm")
  .addEventListener("submit", function (event) {
    event.preventDefault();

    const emailValue = document.querySelector(".login__input--user").value;
    const passwordValue = document.querySelector(
      ".login__input--password"
    ).value;

  if (!isValidEmail(emailValue)) {
   toastr["warning"]("登入失敗", "Email格式不正確");
    return;
  }

    let loginData = {
      email: emailValue,
      password: passwordValue,
    };

    $.ajax({
      url: `${baseUrl}Account/login`,
      type: "POST",
      contentType: "application/json",
      data: JSON.stringify(loginData),
      success: (res) => {
        // Handle the success response
        console.log(res);
        localStorage.setItem("token", res.token);
        toastr["success"]("歡迎回來!", "成功登入");
        wrapper.classList.remove("active-popup");
        setTimeout(function () {
          window.location.reload();
        }, 1000);
      },
      error: (xhr, status, error) => {
        if(xhr.status === 401)
        {
          toastr["warning"]("登入失敗", "帳號或密碼不正確");
          return;
        }
      },
    });


  });


//Close Form
iconClose.addEventListener("click", () => {
  overlay.style.display = "none";
  wrapper.classList.remove("active-popup");
  window.location.href = "https://cofstyle.shop/list/Activity-List.html"; 
});


//Register Form
document
  .getElementById("registerForm")
  .addEventListener("submit", function (event) {
    event.preventDefault();

    // Get input values
    const registerUserNameValue = document.querySelector(
      ".register__input--user"
    ).value;
    const registerEmailValue = document.querySelector(
      ".register__input--email"
    ).value;
    const registerPasswordValue = document.querySelector(
      ".register__input--password"
    ).value;
    const mainImageInput = document.getElementById("main_image");

    const emailError = document.getElementById("emailError");

  if (!isValidEmail(registerEmailValue)) {
    emailError.textContent = "Email 格式不符";
    event.preventDefault(); // Prevent the form from submitting
    return;
  } else {
    emailError.textContent = "";
  }

    // Create an array to store error messages
    const errorMessages = [];

    // Check password requirements one by one
    if (!/(?=.*\d)/.test(registerPasswordValue)) {
      errorMessages.push("密碼至少一個數字.");
    }

    if (!/(?=.*[a-z])/.test(registerPasswordValue)) {
      errorMessages.push(
        "至少一個小寫字元."
      );
    }

    if (!/(?=.*[A-Z])/.test(registerPasswordValue)) {
      errorMessages.push(
        "至少一個大寫字元."
      );
    }

    if (registerPasswordValue.length < 6 || registerPasswordValue.length > 8) {
      errorMessages.push("密碼包含數字需要6~8個字元.");
    }
    // Display error messages if there are any
    const passwordError = document.getElementById("passwordError");
    if (errorMessages.length > 0) {
      passwordError.textContent = errorMessages.join(" ");
      event.preventDefault(); // Prevent the form from submitting
      return;
    } else {
      passwordError.textContent = ""; // Clear any previous error message
    }



    const formData = new FormData();
    formData.append("email", registerEmailValue);
    formData.append("password", registerPasswordValue);
    formData.append("showName", registerUserNameValue);
    formData.append("userName", registerUserNameValue);
    formData.append("MainImage", mainImageInput.files[0]);

    console.log(formData);

    $.ajax({
      url: `${baseUrl}Account/register`,
      type: "POST",
      processData: false, // Prevent jQuery from processing the data
      contentType: false, // Set content type to false to let the server handle it as multipart
      data: formData,
      success: (res) => {
        // Handle the success response
        console.log(res + "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!1");
        localStorage.setItem("token", res.token);
        toastr["success"]("歡迎成為會員!", "成功註冊");
        wrapper.classList.remove("active-popup");
        setTimeout(function () {
          window.location.reload();
        }, 3000);
      },
      error: (xhr, status, error) => {
        // Handle the error response
        console.log(error);
        toastr["error"]("", "該用戶已註冊過");
      },
    });

  });

