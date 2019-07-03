// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function SendVisitorBehavior()
{
    var pageName = window.location.pathname.substring(window.location.pathname.lastIndexOf('/') + 1);
    var browserInfo = navigator.userAgent;
    var indexOfParams = location.href.indexOf('?');
    var pageParams = indexOfParams > -1 ? location.href.substring(indexOfParams + 1) : '';

    var behaviorInfo = {
        "ip": "",
        "pageName": pageName,
        "browserName": browserInfo,
        "pageParams": pageParams
    };

    $.ajax({
        url: '/api/Analytics',
        type: 'post',
        datatype: 'json',
        contentType: 'application/json',
        data: JSON.stringify(behaviorInfo),
        success: function (data) { console.log('User Behavior has been sent.'); }
    });


}

$(document).ready(SendVisitorBehavior);