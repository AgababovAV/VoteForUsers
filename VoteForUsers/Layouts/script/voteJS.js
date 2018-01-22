(function () {
    if (!_spBodyOnLoadCalled) {
        _spBodyOnLoadFunctions.push(pageLoad);
    } else {
        pageLoad();
    }

    function pageLoad() {
        move();
    }
})();

function sendCommentBestEmp(id) {
    var getUserLogin = document.getElementById(id);
    if (getUserLogin.className == "activeUser") {
        getUserLogin.className = "notActive";
        var displayNameFromInput = $("#" + id).find("p.displayName")[0].innerText;
        var departmentFromInput = $("#" + id).find("p.department")[0].innerText;
        var emailtFromInput = $("#" + id).find("input[type='hidden']")[0].value;
        $("#userFIO").val("");
        $("#userDepartment").val("");
        $("#userEmail").val("");

    } else {
        $("table").find("div").removeClass("activeUser").addClass("notActive");
        getUserLogin.className = "activeUser";
        SetInputValue(id);
    }

}


function SetInputValue(id) {
    var displayNameFromInput = $("#" + id).find("p.displayName")[0].innerText;
    var departmentFromInput = $("#" + id).find("p.department")[0].innerText;
    var emailtFromInput = $("#" + id).find("input[type='hidden']")[0].value;
    $("#userFIO").val(displayNameFromInput);
    $("#userDepartment").val(departmentFromInput);
    $("#userEmail").val(emailtFromInput);
    

}



function move() {
    var findAllDivInTable = $("table td div");
    var arrayObject = [];

    for (var i = 0; i < findAllDivInTable.length; i++) {
        if (findAllDivInTable[i].id.indexOf("percent") != 0) {
            //console.log(findAllDivInTable[i].id + " нет");
        } else {
            //console.log(findAllDivInTable[i].id + " есть");
            var percent = document.getElementById(findAllDivInTable[i].id);
            percent.style.position = "absolute";
            percent.style.bottom = 0;
            percent.style.right = 0;
            percent.style.top = -2;
            percent.style.fontSize = "12px";
            percent.style.color = "#8994a3";
            var startSubstringPercent = percent.id.indexOf("-")+ 1;
            var endSubstringPercent = percent.id.length;
            var progBar = document.getElementById("progressBar" + percent.id.substring(startSubstringPercent, endSubstringPercent));
            var currentScore = percent.innerHTML;
            progBar.style.width = "1%";
            progBar.style.height = "12px";
            progBar.style.backgroundColor = "#f15758";
            arrayObject.push({ sender: progBar, curWidth: 1, targetWidth: currentScore, text: currentScore, percentSender: percent });
        }
    }

    var arrayObjectLength = arrayObject.length;   
    var id = setInterval(function () {
        var done = true;
        for (var i = 0; i < arrayObjectLength; i++) {
            var elem = arrayObject[i];
           
            if (elem.curWidth < elem.targetWidth) {
                elem.curWidth = elem.curWidth + 1;
                done = false;                
                elem.sender.style.width = elem.curWidth + "%";
                elem.percentSender.innerText = elem.curWidth + "%";
                //elem.sender.innerText = elem.curWidth + "%";
                //console.log(elem.sender.id + " | " + elem.curWidth + ' | ' + elem.sender.style.width); 
            } 
        }
        if (done) {
            clearInterval(id);
        }        
    }, 10);
    
}

