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

$('#BroadcastMessages').ready(GetBroadcastMessages);
$('#FAQLogin').ready(GetClientFAQs);
$('#FAQNonLogin').ready(GetGeneralFAQs);