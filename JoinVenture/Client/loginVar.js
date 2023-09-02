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


//Login Form
document
  .getElementById("loginForm")
  .addEventListener("submit", function (event) {
    event.preventDefault();

    // Get input values
    const emailValue = document.querySelector(".login__input--user").value;
    const passwordValue = document.querySelector(
      ".login__input--password"
    ).value;

    // Now you can use emailValue and passwordValue for further processing
    console.log("Email:", emailValue);
    console.log("Password:", passwordValue);

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
      },
      error: (xhr, status, error) => {
        // Handle the error response
        console.log(error);
      },
    });

    wrapper.classList.remove("active-popup");
    setTimeout(function () {
      window.location.reload();
    }, 1000);
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
    const registerUserNameValue = document.querySelector(".register__input--user").value;
    const registerEmailValue = document.querySelector(".register__input--email").value;
    const registerPasswordValue = document.querySelector(".register__input--password").value;
    const mainImageInput = document.getElementById("main_image");

    // Now you can use emailValue and passwordValue for further processing
    console.log("UserName:", registerUserNameValue);
    console.log("EMail:", registerEmailValue);
    console.log("Password:", registerPasswordValue);


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
        console.log(res+"!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!1");
        localStorage.setItem("token", res.token);
        toastr["success"]("歡迎成為會員!", "成功註冊");
      },
      error: (xhr, status, error) => {
        // Handle the error response
        console.log(error);
        toastr["error"]("錯誤", "註冊失敗");
      },
    });

    wrapper.classList.remove("active-popup");
    setTimeout(function () {
      window.location.reload();
    }, 2000);
  });

