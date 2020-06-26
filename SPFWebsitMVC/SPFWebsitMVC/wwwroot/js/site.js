// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function GetBroadcastMessages() {
    $('#BroadcastMessages').html('');
    $.ajax({
        url: 'https://localhost:44391/api/broadcastmessages/GetCurrentBroadcastMessages',
        dataType: 'json',
        type: 'get',
        contentType: 'application/json',
        success: function (data) {
            $('#BroadcastMessages').html('<h5 id = "AppendMessage"></h5>');
            $.each(data, function (index) {
                $('#AppendMessage').append('<h6>', data[index].message, '</h6>');
            })
        },
        error: function (jqXhr, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}

function GetClientFAQs() {
    $('#FAQLogin').html('');
    $.ajax({
        url: 'https://localhost:44391/api/FAQs/GetClientFAQs',
        dataType: 'json',
        type: 'get',
        contentType: 'application/json',
        success: function (data) {
            $('#FAQLogin').html('<h5 id = "AppendClientFAQ"></h5>');
            $.each(data, function (index) {
                $('#AppendClientFAQ').append('<h6>', 'Q: ', data[index].question, '</h6>', '<h6>', 'A: ', data[index].answer, '</h6><br/><br/>');
            })
        },
        error: function (jqXhr, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}

function GetGeneralFAQs() {
    $('#FAQNonLogin').html('');
    $.ajax({
        url: 'https://localhost:44391/api/FAQs/GetGeneralFAQs',
        dataType: 'json',
        type: 'get',
        contentType: 'application/json',
        success: function (data) {
            $('#FAQNonLogin').html('<h5 id = "AppendGeneralFAQ"></h5>');
            $.each(data, function (index) {
                $('#AppendGeneralFAQ').append('<h6>', 'Q: ', data[index].question, '</h6>', '<h6>', 'A: ', data[index].answer, '</h6><br /><br />');
            })
        },
        error: function (jqXhr, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}

function GetFavVideo1() {
    if ($('#favVid1').val() != 0) {
        $('.FavoriteVideo1').html('');
        $.ajax({
            url: 'https://localhost:44391/api/Videos/' + $('#favVid1').val(),
            dataType: 'json',
            type: 'get',
            contentType: 'application/json',
            success: function (data) {
                var dict = {
                    VideoId: data.videoId,
                    Name: data.name,
                    Topic: data.topic,
                    Link: data.link,
                    DatePosted: data.datePosted,
                    CurrentUse: data.currentUse,
                    TotalUse: data.totalUse,
                    Post: data.post
                };

                $('.FavoriteVideo1').html('<video width="400px" controls><source src='+ dict.Link +' type="video/mp4" /></video>');
            },
            error: function (jqXhr, textStatus, errorThrown) {
                console.log(errorThrown);
            }
        });
    }
}

function GetFavVideo2() {
    if ($('#favVid2').val() != 0) {
        $('.FavoriteVideo2').html('');
        $.ajax({
            url: 'https://localhost:44391/api/Videos/' + $('#favVid2').val(),
            dataType: 'json',
            type: 'get',
            contentType: 'application/json',
            success: function (data) {
                var dict = {
                    VideoId: data.videoId,
                    Name: data.name,
                    Topic: data.topic,
                    Link: data.link,
                    DatePosted: data.datePosted,
                    CurrentUse: data.currentUse,
                    TotalUse: data.totalUse,
                    Post: data.post
                };

                $('.FavoriteVideo2').html('<video width="400px" controls><source src=' + dict.Link + ' type="video/mp4" /></video>');
            },
            error: function (jqXhr, textStatus, errorThrown) {
                console.log(errorThrown);
            }
        });
    }
}

function GetFavVideo3() {
    if ($('#favVid3').val() != 0) {
        $('.FavoriteVideo3').html('');
        $.ajax({
            url: 'https://localhost:44391/api/Videos/' + $('#favVid3').val(),
            dataType: 'json',
            type: 'get',
            contentType: 'application/json',
            success: function (data) {
                var dict = {
                    VideoId: data.videoId,
                    Name: data.name,
                    Topic: data.topic,
                    Link: data.link,
                    DatePosted: data.datePosted,
                    CurrentUse: data.currentUse,
                    TotalUse: data.totalUse,
                    Post: data.post
                };

                $('.FavoriteVideo3').html('<video width="400px" controls><source src=' + dict.Link + ' type="video/mp4" /></video>');
            },
            error: function (jqXhr, textStatus, errorThrown) {
                console.log(errorThrown);
            }
        });
    }
}

$('#BroadcastMessages').ready(GetBroadcastMessages);
$('#FAQLogin').ready(GetClientFAQs);
$('#FAQNonLogin').ready(GetGeneralFAQs);
$('#favVid1').ready(GetFavVideo1);
$('#favVid2').ready(GetFavVideo2);
$('#favVid3').ready(GetFavVideo3);