// const accessToken = localStorage.getItem("token");


class NotificationHub {
  constructor() {
    this.comments = [];
    this.hubConnection = null;
  }

  // Method to create the hub connection
  createHubConnection() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${baseUrl}chat`, {
        accessTokenFactory: () => accessToken,
      })
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Information)
      .build();

    this.hubConnection
      .start()
      .catch((error) => console.log("Error establishing connection:", error));


    this.hubConnection.on("Follower Only Messages", (comment, activityNow) => {
      console.log(comment, activityNow);
      var ActivityTitle = activityNow.title;
      AddItemToNotification(comment, ActivityTitle);
      itemCount = dropdownMenu.getElementsByTagName("li").length;

      badge.textContent = itemCount.toString();
    });
  }

  stopHubConnection() {
    this.hubConnection
      .stop()
      .catch((error) => console.log("Error stopping connection: ", error));
  }


}


//Add Notification UI
function AddItemToNotification(comment, ActivityTitle) {
  const dropdownMenu = document.querySelector(".dropdown-menu");
  const listItem = document.createElement("li");
  const link = document.createElement("a");
  link.classList.add("dropdown-item");
  link.href = "#";

  link.textContent = `(${ActivityTitle})${comment.showName} 說 : ${comment.body}`;

  const image = document.createElement("img");
  image.classList.add("img-circle", "img-sm");
  image.alt = "Profile Picture";
  image.src = comment.image;

  image.style.borderRadius = "50%";
  image.style.width = "50px";
  image.style.height = "50px";
  image.style.objectFit = "cover";
  image.style.display = "inline-block";

  link.appendChild(image);

  listItem.appendChild(link);
  dropdownMenu.appendChild(listItem);
}


const notificationHub = new NotificationHub();
notificationHub.createHubConnection();


