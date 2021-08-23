let monday__option = document.getElementById("active__1");
let tuesday__option = document.getElementById("active__2");
let wednesday__option = document.getElementById("active__3");
let thursday__option = document.getElementById("active__4");
let friday__option = document.getElementById("active__5");
let saturday__option = document.getElementById("active__6");
let sunday__option = document.getElementById("active__7");

monday__option.onclick = function () {
    monday__option.style.background = "white";
    monday__option.style.color = "black";
    change__selected_style(tuesday__option, wednesday__option, thursday__option, friday__option, saturday__option, sunday__option)
}
tuesday__option.onclick = function () {
    tuesday__option.style.background = "white";
    tuesday__option.style.color = "black";
    change__selected_style(monday__option, wednesday__option, thursday__option, friday__option, saturday__option, sunday__option)
}
wednesday__option.onclick = function () {
    wednesday__option.style.background = "white";
    wednesday__option.style.color = "black";
    change__selected_style(monday__option, tuesday__option, thursday__option, friday__option, saturday__option, sunday__option)
}
thursday__option.onclick = function () {
    thursday__option.style.background = "white";
    thursday__option.style.color = "black";
    change__selected_style(monday__option, tuesday__option, wednesday__option, friday__option, saturday__option, sunday__option)
}
friday__option.onclick = function () {
    friday__option.style.background = "white";
    friday__option.style.color = "black";
    change__selected_style(monday__option, tuesday__option, wednesday__option, thursday__option, saturday__option, sunday__option)
}
saturday__option.onclick = function () {
    saturday__option.style.background = "white";
    saturday__option.style.color = "black";
    change__selected_style(monday__option, tuesday__option, wednesday__option, thursday__option, friday__option, sunday__option)
}
sunday__option.onclick = function () {
    sunday__option.style.background = "white";
    sunday__option.style.color = "black";
    change__selected_style(monday__option, tuesday__option, wednesday__option, thursday__option, friday__option, saturday__option)
}

function change__selected_style(num1, num2, num3, num4, num5, num6) {
    num1.style.background = "rgba(239, 239, 239, 0.8)";
    num2.style.background = "rgba(239, 239, 239, 0.8)";
    num3.style.background = "rgba(239, 239, 239, 0.8)";
    num4.style.background = "rgba(239, 239, 239, 0.8)";
    num5.style.background = "rgba(239, 239, 239, 0.8)";
    num6.style.background = "rgba(239, 239, 239, 0.8)";
    num1.style.color = "black";
    num2.style.color = "black";
    num3.style.color = "black";
    num4.style.color = "black";
    num5.style.color = "black";
    num6.style.color = "black";
}