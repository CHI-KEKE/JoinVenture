const wrapper = document.querySelector(".wrapper");
const iconClose = document.querySelector(".icon-close");

// const EmailValue = document.querySelector(".login__input--user").value;
// const PasswordValue = document.querySelector(".login__input--password").value;
// const LoginSent = document.querySelector(".login-btn");


//Login Mechanism
function loginNative() {
  wrapper.classList.add("active-popup");
}

function logout(){
  localStorage.removeItem("token");
}


// function login(){

//     let loginData = {
//       email: EmailValue,
//       password: PasswordValue,
//     };

//     $.ajax({
//       url: "http://localhost:5000/api/Account/login",
//       type: "POST",
//       contentType: "application/json",
//       data: JSON.stringify(loginData),
//       success: (res) => {
//         // Handle the success response
//         console.log(res);
//         localStorage.setItem("token", res.token);
//       },
//       error: (xhr, status, error) => {
//         // Handle the error response
//         console.log(error);
//       },
//     });

//     wrapper.classList.remove("active-popup");

// }


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
      url: "http://localhost:5000/api/Account/login",
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
    window.location.reload();
  });



//Close Form
iconClose.addEventListener("click", () => {
  wrapper.classList.remove("active-popup");
});


//Login Invoke Hub to load HostedActivities
