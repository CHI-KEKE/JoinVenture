const activityDetail = document.querySelector(".activity-detail");
const FollowButton = document.querySelector(".follow-btn");
const bookingButton = document.querySelector(".booking-btn");

const queryParams = new URLSearchParams(window.location.search);
const activityId = queryParams.get("id");
const accessToken = localStorage.getItem("token");


//NotificationBell

const badge = document.getElementById("notificationBadge");

const dropdownMenu = document.getElementById("dropdownMenu");

// let itemCount = dropdownMenu.getElementsByTagName("li").length;

// badge.textContent = itemCount.toString();



// Fetch Activity Data
axios
  .get(`http://localhost:5000/api/Activities/${activityId}`)
  .then(function (response) {
    const activity = response.data;

    console.log(response);
    // Populate the template with activity details
    document.querySelector(".activity-title").textContent = activity.title;
    console.log(activity.title);
    document.querySelector(".activity-description").textContent = activity.description;
    document.querySelector(".activity-ticket").textContent = activity.tickets;


    const hostAttendee = activity.hostUserName;


    console.log(hostAttendee);
    if (hostAttendee) {
      document.querySelector(".activity-host").textContent = hostAttendee;
    }

    // document.querySelector(".activity-host").textContent = activity.host;
    const activityDate = new Date(activity.date);
    document.querySelector(
      ".activity-date"
    ).textContent = `${activityDate.getFullYear()}-${activityDate.getMonth() + 1}-${activityDate.getDate()}`;
    document.querySelector(".activity-date-2").textContent = `${activityDate.getFullYear()}-${activityDate.getMonth() + 1}-${activityDate.getDate()}`;
    document.querySelector(".activity-city-venue").textContent = `${activity.city}, ${activity.venue}`;

    const bgContainer = document.querySelector(".activity-image");
    bgContainer.style.backgroundImage = `url('${activity.image}')`;

  })


  .catch(function (error) {
    console.error("Error fetching activity details:", error);
  });



// //Hub

// // const hubConnection = new signalR.HubConnectionBuilder()
// //   .withUrl(`http://localhost:5000/chat?activityId=${activityId}`, {
// //     accessTokenFactory: () => accessToken,
// //   })
// //   .build();


// // hubConnection
// //   .start()
// //   .catch((error) => console.error("Error establishing connection:", error));


class CommentStore {
  constructor() {
    this.comments = [];
    this.hubConnection = null;
  }

  // Method to create the hub connection
  createHubConnection(activityId) {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`http://localhost:5000/chat?activityId=${activityId}`, {
        accessTokenFactory: () => accessToken,
      })
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Information)
      .build();

    this.hubConnection
      .start()
      .catch((error) => console.log("Error establishing connection:", error));

    this.hubConnection.on("LoadComments", (comments) => {
      this.comments = comments;
      // Call a function to update the UI with the received comments
      console.log(comments);
      updateUIWithComments(this.comments);
    });

    this.hubConnection.on("Receive Comments", (comment) => {
      this.comments.push(comment);
      // Call a function to update the UI with the new comment
      addCommentToUI(comment);
    });

    this.hubConnection.on("LoadTicketCount", (initialTicketCount) => {
      // Update the UI with the initial ticket count
      updateTicketCountUI(initialTicketCount);
    });

    this.hubConnection.on("UpdateTicketCount", (updatedTicketCount) => {
      // Update the UI with the updated ticket count
      updateTicketCountUI(updatedTicketCount.value);
    });


    this.hubConnection.on("Follower Only Messages", (comment) => {
      var ActivityTitle = document.querySelector(".activity-title").textContent;
      AddItemToNotification(comment,ActivityTitle);
      itemCount = dropdownMenu.getElementsByTagName("li").length;

      badge.textContent = itemCount.toString();

    });

  }

  

  stopHubConnection() {
    this.hubConnection
      .stop()
      .catch((error) => console.log("Error stopping connection: ", error));
  }

  clearComments() {
    this.comments = [];
    this.stopHubConnection();
  }

  async addComment(newCommentText) {
    try {
      await this.hubConnection.invoke("SendComment", {
        activityId: activityId,
        body: newCommentText,
      });
    } 
    
    catch (error) {
      console.log(error);
    }
  }





async bookTicket(activityId) {
    try {
        await this.hubConnection.invoke("Booking",
        {
           activityId : activityId,
        });
    } 
    
    catch (error) {
        console.log(error);
    }
}


//Testing Group

  async follow(){
    await this.hubConnection.send('FollowActivity');
  }
}
// // Function to update the UI with received comments



function updateUIWithComments(comments){
     const commentSection = document.querySelector(".panel-body");
    commentSection.innerHTML = "";

    // Iterate through comments and update the UI with each comment
    comments.forEach((comment) => {
        const newComment = document.createElement("div");
        newComment.className = "media-block";
        newComment.innerHTML = `
          <a class="media-left" href="#"><img class="img-circle img-sm" alt="Profile Picture" src="${comment.image}"></a>
          <div class="media-body">
              <div class="mar-btm">
                  <a href="#" class="btn-link text-semibold media-heading box-inline">${comment.showName}</a>
                  <p class="text-muted text-sm"><i class="fa fa-mobile fa-lg"></i> - From Mobile - Just now</p>
              </div>
              <p>${comment.body}</p>
              <div class="pad-ver">
                  <div class="btn-group">
                      <a class="btn btn-sm btn-default btn-hover-success" href="#"><i class="fa fa-thumbs-up"></i></a>
                      <a class="btn btn-sm btn-default btn-hover-danger" href="#"><i class="fa fa-thumbs-down"></i></a>
                  </div>
                  <a class="btn btn-sm btn-default btn-hover-primary" href="#">Comment</a>
              </div>
              <hr>
          </div>
      `;
        console.log(newComment);

        commentSection.insertAdjacentElement("afterbegin", newComment);
    });   
}



// // Function to add a new comment to the UI
function addCommentToUI(comment) {
  // console.log(comment);
  const commentSection = document.querySelector(".panel-body");
  const newComment = document.createElement("div");
  newComment.className = "media-block comment-fade";
  newComment.innerHTML = `
    <a class="media-left" href="#"><img class="img-circle img-sm" alt="Profile Picture" src="${comment.image}"></a>
    <div class="media-body">
        <div class="mar-btm">
            <a href="#" class="btn-link text-semibold media-heading box-inline">${comment.showName}</a>
            <p class="text-muted text-sm"><i class="fa fa-mobile fa-lg"></i> - From Mobile - Just now</p>
        </div>
        <p>${comment.body}</p>
        <div class="pad-ver">
            <div class="btn-group">
                <a class="btn btn-sm btn-default btn-hover-success" href="#"><i class="fa fa-thumbs-up"></i></a>
                <a class="btn btn-sm btn-default btn-hover-danger" href="#"><i class="fa fa-thumbs-down"></i></a>
            </div>
            <a class="btn btn-sm btn-default btn-hover-primary" href="#">Comment</a>
        </div>
        <hr>
    </div>
`;

  // Insert the new comment at the beginning of the commentSection
  commentSection.insertBefore(newComment, commentSection.firstChild); 

  setTimeout(() => {
    newComment.classList.add("show");
  }, 10);
}


//Update ticketsCount

function updateTicketCountUI(ticketCount) {
  console.log(ticketCount);
  const ticketCountElement = document.querySelector(".activity-ticket");
  ticketCountElement.textContent = ticketCount;
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

// Usage
const commentStore = new CommentStore();
commentStore.createHubConnection(activityId);






















function addNewComment() {
  const newCommentText = document.querySelector(".newComment").value;
  document.querySelector(".newComment").value = "";
  console.log(newCommentText);
  commentStore.addComment(newCommentText);
}


// Booking

function BookingActivity(){

  BookingTransition()
  
  // bookingButton.classList.add("clicked");
  // setTimeout(() => {
  //   bookingButton.classList.remove("clicked");
  // }, 200); 
  setTimeout(BookingRecover, 500);

  commentStore.bookTicket(activityId);

  window.location.href = `https://fff5-2402-7500-4d5-a113-e930-21d7-3d9c-cf18.ngrok-free.app/Client/Ticket-Selecting.html?id=${activityId}`;
}

//Booking Transition

function BookingTransition(){
  bookingButton.style.backgroundColor = "#7D7C7A";
  bookingButton.innerText = "o(´^｀)o";
}

//Booking Recover

function BookingRecover() {
  bookingButton.style.backgroundColor = "#484D6D";
  bookingButton.innerText = "訂票排假ヽ( ຶ▮ ຶ)ﾉ";
}

//Following

function FollowActivity(){
  commentStore.follow();
  FollowButton.disabled = true;
  FollowButton.innerText = "已追蹤";
  FollowButton.style.backgroundColor = "#A0A0A0";
  FollowButton.style.borderColor = "#A0A0A0";
}
