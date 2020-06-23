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

$('#BroadcastMessages').ready(GetBroadcastMessages);