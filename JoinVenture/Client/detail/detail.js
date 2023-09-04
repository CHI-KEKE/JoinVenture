const activityDetail = document.querySelector(".activity-detail");
const FollowButton = document.querySelector(".follow-btn");
const bookingButton = document.querySelector(".booking-btn");

const queryParams = new URLSearchParams(window.location.search);
const activityId = queryParams.get("id");


// Fetch Activity Data
axios
  .get(`${baseUrl}Activities/${activityId}`)
  .then(function (response) {
    const activity = response.data;

    let ticketCounts = 0;
    for (const ticketPackage of activity.ticketPackages) {
      for (const ticket of ticketPackage.tickets) {
        if (ticket.status === "Available") {
          ticketCounts++;
        }
      }
    }

    console.log(response);

    document.querySelector(".activity-title").textContent = activity.title;
    console.log(activity.title);
    document.querySelector(".activity-description").textContent =
      activity.description;
    document.querySelector(".activity-ticket").textContent = ticketCounts;

    const hostAttendee = activity.hostUserName;

    if (hostAttendee) {
      document.querySelector(".activity-host").textContent = hostAttendee;
    }

    const activityDate = new Date(activity.date);
    document.querySelector(
      ".activity-date"
    ).textContent = `${activityDate.getFullYear()}-${
      activityDate.getMonth() + 1
    }-${activityDate.getDate()}`;
    document.querySelector(
      ".activity-date-2"
    ).textContent = `${activityDate.getFullYear()}-${
      activityDate.getMonth() + 1
    }-${activityDate.getDate()}`;
    document.querySelector(
      ".activity-city-venue"
    ).textContent = `${activity.city}, ${activity.venue}`;

    const bgContainer = document.querySelector(".activity-image");
    bgContainer.style.backgroundImage = `url('${activity.image}')`;
  })

  .catch(function (error) {
    console.error("Error fetching activity details:", error);
  });

////////////////////////////////////////////////////////Hub///////////////////////////////////////////////////

class CommentStore {
  constructor() {
    this.comments = [];
    this.hubConnection = null;
  }

  // Method to create the hub connection
  createHubConnection(activityId) {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${baseUrl}chat?activityId=${activityId}`, {
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
    } catch (error) {
      console.log(error);
    }
  }

  async follow(accessToken) {
    await this.hubConnection.send("FollowActivity", accessToken);
  }
}

////////////////////////////////////////////////////////Hub///////////////////////////////////////////////////




// // Function to update the UI with received comments

function updateUIWithComments(comments) {
  const commentSection = document.querySelector(".panel-body");
  commentSection.innerHTML = "";

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




// Usage
const commentStore = new CommentStore();
commentStore.createHubConnection(activityId);

//NewComment

function addNewComment() {
  if (accessToken != null) {
    const newCommentText = document.querySelector(".newComment").value;
    document.querySelector(".newComment").value = "";
    console.log(newCommentText);
    commentStore.addComment(newCommentText);
  } else {
    openPopup();
  }
}

// Booking

function BookingActivity() {
  BookingTransition();

  setTimeout(BookingRecover, 500);

  window.location.href = `https://cofstyle.shop/ticket-selecting/Ticket-Selecting.html?id=${activityId}`;
}

//Booking Transition

function BookingTransition() {
  bookingButton.style.backgroundColor = "#7D7C7A";
  bookingButton.innerText = "o(´^｀)o";
}

//Booking Recover

function BookingRecover() {
  bookingButton.style.backgroundColor = "#484D6D";
  bookingButton.innerText = "訂票排假ヽ( ຶ▮ ຶ)ﾉ";
}

//Following

function FollowActivity() {
  $.post({
    url: baseUrl + `Activities/${activityId}/follow`,
    dataType: "json",
    contentType: "application/json",
    beforeSend: function (xhr) {
      // Set the Authorization header with the JWT token
      xhr.setRequestHeader("Authorization", "Bearer " + accessToken);
    },

    success: (res) => {
      console.log(res);
    },

    error: (err) => {
      console.log(err);
    },
  });

  FollowButton.disabled = true;
  FollowButton.innerText = "已追蹤";
  FollowButton.style.backgroundColor = "#A0A0A0";
  FollowButton.style.borderColor = "#A0A0A0";

  commentStore.follow(accessToken);
}
