



var audioPlayer = $("#audioPlayer").first()[0];
var nextBtn = $("#nextBtn");
var prevBtn = $("#prevBtn");
var playBtn = $("#playBtn");
var pauseBtn = $("#pauseBtn");


$(audioPlayer).on("timeupdate", function (event) {
    var currentTime = this.currentTime;
    var length = this.duration;
    $("#progressIndeicator").css("width", (currentTime * 100 / length) + "%");

    console.log(this.currentTime, this.duration)
});

function playAudio(id) {
    playBtn.hide();
    pauseBtn.show();
    audioPlayer.src = "/MusicTracks/GetAudio/" + id;
    audioPlayer.play();
}

function pauseAudio() {

    playBtn.show();
    pauseBtn.hide();

    audioPlayer.pause();
}

$(".trackCard").click(function () {
    $(".trackCard").removeClass("trackCardSelected");
    $(this).addClass("trackCardSelected");

    var trackID = $(this).data("trackid");
    playAudio(trackID);
});

$(playBtn).click(function () {
    var selectedTrackId = $(".trackCardSelected").data("trackid");
    console.log(selectedTrackId);
    playAudio(selectedTrackId);
});

$(pauseBtn).click(function (e) {

    pauseAudio();
});