var GetUserUrl = `${baseUrl}Account`;
const overlay = document.querySelector(".overlay");
const wrapper2 = document.querySelector(".wrapper");

const openPopup = function () {
  wrapper2.classList.add("active-popup");
  overlay.style.display = "block";
};

const closePopup = function () {
  wrapper2.classList.remove("active-popup");
  overlay.style.display = "none";
};

if (!accessToken) {
  openPopup();
}
