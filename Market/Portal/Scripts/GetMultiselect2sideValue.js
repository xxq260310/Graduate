function getselectedtext()
{
    var right = document.getElementById("MultiSelect2sidems2side__dx");
    var str = "";
        for (i = right.length - 1; i >= 0; i--)
        {
            str += right.options[i].value + ",";
        }
    var optionstext = document.getElementById("selected-options");
    optionstext.value = str;
}