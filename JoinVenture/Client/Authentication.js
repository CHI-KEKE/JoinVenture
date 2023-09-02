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
        if (res.photos[0] != undefined) {
          userImage.innerHTML = `<img src="${res.photos[0].url}" alt="User Image" width="30" height="30" class = "rounded-circle" style="object-fit:cover;object-position: center;">`;
          userImage.style.display = "block";
        }

        // Show the logout button and hide the login button
        const logoutButton = document.getElementById("logoutButton");
        logoutButton.style.display = "block";
        const loginButton = document.getElementById("loginButton");
        loginButton.style.display = "none";
      },
      error: (xhr, status, error) => {
        if (err.status === 401)
        {
          console.error("just..not login user now");
        } 
      },
    });
  }
});

//Login Mechanism
function loginNative() {
  wrapper.classList.add("active-popup");
}

function logout() {
  localStorage.removeItem("token");
  setTimeout(function () {
    window.location.reload();
  }, 1000);
}

