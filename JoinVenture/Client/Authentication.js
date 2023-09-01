const wrapper = document.querySelector(".wrapper");
const iconClose = document.querySelector(".icon-close");
// const EmailValue = document.querySelector(".login__input--user").value;
// const PasswordValue = document.querySelector(".login__input--password").value;
// const LoginSent = document.querySelector(".login-btn");


//To Check if user is Authenticated 

$(document).ready(function () {
  const getUserUrl = `${baseUrl}Account`;

  if (accessToken) {
    
    $.ajax({
      url: getUserUrl,
      type: "GET",
      dataType: "json",
      contentType: "application/json",
      beforeSend: function (xhr) {
        xhr.setRequestHeader("Authorization", "Bearer " + accessToken);
      },
      success: (res) => {

        const userImage = document.getElementById("userImage");
        userImage.innerHTML = `<img src="${res.photos[0].url}" alt="User Image" width="30" height="30" class = "rounded-circle" style="object-fit:cover;object-position: center;">`;
        userImage.style.display = "block";

        // Show the logout button and hide the login button
        const logoutButton = document.getElementById("logoutButton");
        logoutButton.style.display = "block";
        const loginButton = document.getElementById("loginButton");
        loginButton.style.display = "none";

      },
      error: (xhr, status, error) => {
        console.error(error);
      },
    });
  }
});



//Login Mechanism
function loginNative() {
  wrapper.classList.add("active-popup");
}

function logout(){
  localStorage.removeItem("token");
    setTimeout(function () {
      window.location.reload();
    }, 1000);
}



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


//Login Invoke Hub to load HostedActivities
