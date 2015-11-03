var DEBUG_MODE = false;
$.fn.serializeObject = function(){
    var o = {};
    var a = this.serializeArray();
    $.each(a, function() {
        if (o[this.name] !== undefined) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};

// this variable should exist per UIWebView and
// cache seralized data for inactive screens (no need to reserialize if data is not changed)
var cacheData = "";

//---------------------------------------------------------------------
function getData(formatted){
	var dict = {
		'Patient': $('#patient_info').serializeObject(),
		'Quality': $('#quality_info').serializeObject(),
		'SatisfactionSurvey': $('#patient_satisfaction_info').serializeObject(),
		'NurseSignature': $('.nurse_sig').serializeObject(),
		'PatientSignature': $('.patient_sig').serializeObject()
	};
    return JSON.stringify(dict,null,((formatted)?'\t':''));
}

//---------------------------------------------------------------------
// Get list of all possible data entry fields
// categorize them and construct json array such as
// [
// {"Name":"Blindness","Val":"ON","Notes":"Hello World","HL":"Y","Type":"PastMedialHistory"},
// {"Name":"Blindness2","Val":"ON","Notes":"Hello World","HL":"Y","Type":"PastMedialHistory"},
// {"Name":"Blindness3","Val":"ON","Notes":"Hello World","HL":"Y","Type":"PastMedialHistory"},	
// ]
function getDataSupportDynamicRows(formatted){
	var retArr = new Array();
	var dict = {};
	var dataArr = $('form').serializeObject();
				
	for (var key in dataArr){
		var val = dataArr[key];
        
		//	$.each($('form').serializeObject(),function(key,val){
		// suggestion?
		// patientInformationItems.[Person Providing Info].ddl -> patientInformationItems_PersonProvidingInfo_ddl
		// e.g.
		// Type.[Name].UI
		// medicalHistoryItems.[Pneumonia].cb
		// Type-Name-UI
		// medicalHistoryItems-Pneumonia-cb
		//
        // use underscore for spaces		
			
        var delimiter = "-";
        
		if (Array.isArray(val)){
			//console.log("duplicated? : " + key);
		}
		else if (val != "" && key.indexOf(delimiter) == -1){
			//console.log("no segments: " + key);
		}
		else if (val != "" && key.indexOf(delimiter) != -1){
			var parts = key.split(delimiter);
			if (parts && parts.length == 3) {
				var name = parts[1];//.replace(/\[/,"").replace(/\]/,"");				
				var type = parts[0];
				var ui = parts[2];								
			
				if (ui == "cb" && val == "on"){
					val = "ON";
				}
			
				var tmpKey = name+type;
				if ( !(tmpKey in dict) ) {
					dict[tmpKey] = {};
				}
				
				dict[tmpKey]["name"] = name;
				dict[tmpKey]["type"] = type;
								
				if (ui == "ta"){
					dict[tmpKey]["notes"] = val;
				}
				else if (ui == "hl"){
					dict[tmpKey]["hl"] = val;
				}
				// ddl,cb etc...?
				else {
					dict[tmpKey]["val"] = val;
				}								
			}
			else {
				//console.log("too many segments: " + key);							
			}
		}
//	});
	}
	$.each(dict,function(key,row){
		if ( !("val" in row) ){
			row["val"] = "";
		}
		if ( !("notes" in row) ){
			row["notes"] = "";
		}
		if ( !("hl" in row) ){
			row["hl"] = "";
		}
		
		if (row["val"] != "" || row["notes"]!="" || row["hl"]!=""){
			retArr.push(row);
		}
		
		//console.log(key+","+val);
	});
//	console.log(retArr);
    return JSON.stringify(retArr,null,((formatted)?'\t':''));
}

//---------------------------------------------------------------------------------------------
// Prettyfied
function getDataDebug(){
    return JSON.stringify($('form').serializeObject(),null, '\t').replace(/"on"/g,"\"ON\"");
}                           

//---------------------------------------------------------------------
// This function is used only for postop at this point. The Signature should be blocked in request method.
function finalizeAllIncludingSingatures(dynamicRows){
//    finalizeAll();                                               
    cacheData = getData(dynamicRows);// keep snapshot before freeze
    
    $("input,select,textarea,button").prop("disabled",true);
    //$("input,select,textarea,button").not('.no_freeze').prop("readonly",true);
    $('.sigWrapper').click(false);
    $("img,a").prop("onclick", false);
    $(".popit").hide();
}

//---------------------------------------------------------------------
function finalizeAll(dynamicRows){
    
    cacheData = getData(dynamicRows);// keep snapshot before freeze
    
	$("input,select,textarea,button").not('.no_freeze').prop("disabled",true);
    //$("input,select,textarea,button").not('.no_freeze').prop("readonly",true);
	$('.sigWrapper').not('.no_freeze').click(false);
    $("img,a").not('.no_freeze').prop("onclick", false);
    $(".popit").hide();
}

//---------------------------------------------------------------------
function unlockAll(){
    
    cacheData = "";// destroy the cache
    
	$("input,select,textarea,button").not('.no_freeze').prop("disabled",false);
    //$("input,select,textarea,button").not('.no_freeze').prop("readonly",false);
	$('.sigWrapper').not('.no_freeze').click(true);
    $("img,a").not('.no_freeze').prop("onclick", true);
    $(".popit").show();
}

//---------------------------------------------------------------------
function clearCache(){
    cacheData = "";
}

//---------------------------------------------------------------------
//Is the variable a number?
function isNumber(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
}

//---------------------------------------------------------------------
function pad(num, size) {
    var s = num+"";
    while (s.length < size) s = "0" + s;
    return s;
}
//---------------------------------------------------------------------
// BMI - calculate BMI and return formatted string return blank if malformatted input
function getBMIValue(ht,wt,htUnit,wtUnit){
    if (ht == 0 || wt == 0 || ht == "" || wt == ""){
        return "";
    }
    
    if(htUnit == 'm'){
        ht = ht / .0254;
    }
    if(wtUnit == 'kg'){
        wt = wt / .45359;
    }
    var bmi = (wt/ (ht * ht)) * 703;
    if(isNumber(bmi)){
        return parseFloat(bmi).toFixed(2);
    }
    return "";
}

//---------------------------------------------------------------------
function createNotes(anesStartTime,markers){
	$("#tbAnesthesiaNotes").val("");
	$("#tbAddMeds").val("");
	
	var startTime=null;
	
	if(anesStartTime!=""){
        //debugger; // debugger means break point
		startTime=new Date("1/1/1900 "+anesStartTime);
		var minuteUnit;
        var startHour;
        
        // If the anes start time is less than 15 min, retrack back to last hour's 45's.
        // 45 means minuteUnit == 3
		if(startTime.getMinutes()<15){
			minuteUnit=3;
            startHour = startTime.getHours() - 1;
		}
		else{
			minuteUnit=Math.floor((startTime.getMinutes()-15)/15.0);
            startHour = startTime.getHours();
		}
		startTime=new Date("1/1/1900 "+startHour+":"+minuteUnit*15);
	}
	//var str="<table class='innerTable'>";
	//str=str+"<tr><th>Time</th><th>Notes</th></tr>";
	var noteArr=new Array();
	var drugArr=new Array();
	for(var i=0;i<markers.length;i++){
        
		var marker=markers[i];
        // Note
		if(marker.marker_type==10){
            var arr = marker.marker_value.split("|");
			var n=marker.marker_value.indexOf("|");
			var note="";
            if (arr.length > 1){
                note = arr[1];
            }
            var additional = 0;
            if (arr.length > 2){
                additional = parseInt(arr[2]) * 60000;
            }
			
			var time="";
			
			if(startTime!=null){
				var noteTime=new Date(startTime.getTime()+marker.marker_col*5*60000 + additional);
				time=pad(noteTime.getHours(),2)+":"+pad(noteTime.getMinutes(),2);
			}
			
			//str=str+"<tr><td>"+time+"</td><td>"+note+"</td></tr>";
            if (note){
                note = note.replace("(null)","").replace("<null>","").trim();
            }
			noteArr.push(time+" - "+note);
		}
        // Drug
		else if(marker.marker_type==12){
            var arr = marker.marker_value.split("|");
			var note="";
            if (arr.length > 1){
                note = arr[1];
            }
            var additional = 0;
            if (arr.length > 2){
                additional = parseInt(arr[2]) * 60000;
            }

			
			var time="";
			
			if(startTime!=null){
				var noteTime=new Date(startTime.getTime()+marker.marker_col*5*60000 + additional);
				time=pad(noteTime.getHours(),2)+":"+pad(noteTime.getMinutes(),2);
			}
			
			//str=str+"<tr><td>"+time+"</td><td>"+note+"</td></tr>";
            if (note){
                note = note.replace("(null)","").replace("<null>","").trim();
            }
			drugArr.push(time+" - "+note);
		}
	}
	
	noteArr.sort();
	drugArr.sort();
	
	
	
	$("#tbAnesthesiaNotes").val(noteArr.join('\n'));
	$("#tbAddMeds").val(drugArr.join('\n'));
	
}

//---------------------------------------------------------------------------------------------
function openCamera(tag){
	window.location = "ipro://openCamera/"+tag;
    
}

//---------------------------------------------------------------------------------------------
function openPhoto(tag){
	window.location = "ipro://openPhoto/"+tag;    
}


//--------------------------------------------------------------------------------------------------------------
function openMiniLogin(elementName,index) {
    var scrollTop = $(window).scrollTop();
    var elemWidth=$("#"+elementName).width();
    var elemHeight=$("#"+elementName).height();
    
    var left=$("#"+elementName).position().left;
    var top=$("#"+elementName).position().top;
    var val=$("#"+elementName).val();
    window.location = "ipro://openMiniLogin/"+elementName+","+(left+(elemWidth/2))+","+(top-scrollTop+elemHeight)+"," + index;
}

//---------------------------------------------------------------------------------------------
// this is used in add patient ... should be unified into getData() function
//function getDataFromForm(){
//	
//	var form = $('form').serialize();
//	$('select[disabled]').each( function() {
//                               form = form + '&' + $(this).attr('name') + '=' + $(this).val();
//                               });
//	form += '&SurgeonName=' + encodeURIComponent($("#Surgeon_label").text());
//	return form;
//}

//---------------------------------------------------------------------------------------------
function isDrugNameFilled(){
    if ($("#tbAntiBioDrug1").val() == "" && $("#tbAntiBioDrug2").val() == "" && $("#tbAntiBioDrug3").val() == "" && $("#tbAntiBioDrug4").val() == "") {
        return "N";
    }
    return "Y";
}

//---------------------------------------------------------------------------------------------
// If you need pass the list of preop checkboxes, call this function and print pdf
function debugPreopExpandAll(){
	var selector = "#preop_history";
	$('.modal-content').each(function(){
		$(this).find('.modal-header h4').clone().appendTo(selector);
		$(this).find('.modal-body').clone().appendTo(selector);
	});
	$(selector + ' .modal-body').removeClass();
	$("#preOpreviewList").hide();
}

//---------------------------------------------------------------------------------------------
function debugPreop(){
    var errorIdStr = "";
    $("input,textarea").each(function(){
		var idstr = $(this).attr("id");
		// skip undefined
		if (idstr){
			if (idstr.indexOf(".") != -1) {
				errorIdStr += "ERROR (Dot Detected): " + idstr + "\n";
			}
			else if (idstr.indexOf("(")!=-1){
				errorIdStr += "ERROR (paran Detected): " + idstr + "\n";
			}
			else if (idstr.indexOf(")")!=-1){
				errorIdStr += "ERROR (paran Detected): " + idstr + "\n";
			}
			else if (idstr.indexOf("\\")!=-1){
				errorIdStr += "ERROR (backslash Detected): " + idstr + "\n";
			}
			else if (idstr.indexOf("/")!=-1){
				errorIdStr += "ERROR (forwardlash Detected): " + idstr + "\n";
			}
			else if (idstr.indexOf(" ")!=-1){
				errorIdStr += "ERROR (space Detected): " + idstr + "\n";
			}
			else if (idstr.indexOf(",")!=-1){
				errorIdStr += "ERROR (comma Detected): " + idstr + "\n";
			}
			else if (idstr.indexOf("&")!=-1){
				errorIdStr += "ERROR (amp Detected): " + idstr + "\n";
			}
            else if (idstr.indexOf("+")!=-1){
                errorIdStr += "ERROR (plus Detected): " + idstr + "\n";
            }
            else {
                if(idstr.indexOf("-cb")!=-1){
                	console.log(idstr+","+$("*[for="+idstr+"]").text());
				}
            }
		}
                             
    });
    
    if (errorIdStr != "") {
        console.log("=====[ERRORS]======");
        console.log(errorIdStr);
    }
}

//---------------------------------------------------------------------------------------------
function clearAll(){
    $("input[type=checkbox]").removeAttr("checked");
    $("input[type=text]").val("");
    $("input[type='number']").val("");
    $("input[type=radio]").removeAttr("checked");
    $("select").val('');
}

//---------------------------------------------------------------------------------------------
function fillPreOp(allObj){
	fillData(allObj["results"]["PreOPInfo"]);
    
	bindAllPreop();// defined in preop.html
}

//---------------------------------------------------------------------------------------------
function fillPreOpNew(allObj){
	fillDataDynamicRows(allObj["results"]["NewPreOPInfo"]);
    
	bindAllPreop();// defined in preopnew.html
}

//---------------------------------------------------------------------------------------------
function fillNewPreOpFromHistory(allObj, procId){
    var items = allObj.results.NewPreOPInfo;
    var filterItems = new Array();
    for (var i=0; i<items.length; i++) {
		//        console.log(items[i]["ProcID"] + " " + procId);
        if (items[i]["ProcID"] == procId){
            filterItems.push(items[i]);
        }
    }
    fillDataDynamicRows(filterItems);
    
    // Clear Day of Surgury Section
    $("#dosn_panel input[type=text], #dosn_panel textarea").val("");
    $("#dosn_panel input[type=checkbox]").prop("checked",false);
    bindAllPreop();
}

//---------------------------------------------------------------------------------------------
function fillPreOpFromHistory(allObj, procId){
    var items = allObj.results;
    for (var i=0; i<items.length; i++) {
        if (items[i]["ProcID"] == procId){
            fillData(items[i]);
            bindAllPreop();
            return;
        }
    }
}

//---------------------------------------------------------------------------------------------
function fillCheckList(allObj){
	fillData(allObj["results"]["ProcCheckList"]);
    
    
    //ddlRegional (hidden) is coming in csv format
    //"ddlRegional":"1,2,3,4,5"
    // and we will populate ddlRegional2 (multiselect) from the csv
    // This checkbox require to serialize / deserialize from csv format
    // Our Developer wants to go for quick and dirty way 
    //var data="1,2,3,4";
	if (allObj["results"]["ProcCheckList"]["ddlRegional"] != null){
    	var data = allObj["results"]["ProcCheckList"]["ddlRegional"];
    	var dataarray=data.split(",");
    	$("#ddlRegional2").val(dataarray);
	}
    
    bindAllChecklist(); // defined in checklist.html
}

//---------------------------------------------------------------------------------------------
function fillPacu(allObj){
	fillData(allObj["results"]["PacuVital"]);
    
    changePACUBGColor();
    checkOtherBoxes();
	bindMultiSelect('ddlPACUTransfer2','ddlPACUTransfer');
	bindMultiSelect('ddlICUTransfer2','ddlICUTransfer');
	bindPACUTransfer();
	bindICUTransfer();
}

//---------------------------------------------------------------------------------------------
function fillNote(allObj){    
	$("#tbNotes").val(allObj["results"]["tbNotes"].replace(/\[BR\]/g,"\n"));
	$("#billing_notes").val(allObj["results"]["tbBillingNotes"].replace(/\[BR\]/g,"\n"));
}

//---------------------------------------------------------------------------------------------
function fillPostEval(allObj){
	fillData(allObj["results"]["Postop"]);
}

//---------------------------------------------------------------------------------------------
function fillOB(allObj){
	fillData(allObj["results"]["OBCheckList"]);
	validateOB();
    highlightEpidural();
	bindMultiSelect('tbSiteEpidural2','tbSiteEpidural'); // says tb but ddl
}

//---------------------------------------------------------------------------------------------
// Call this function exclusively from web data.
// Local cache might not contain what we are looking for.
function fillAddendumFromWeb(allObj){
    var arr = allObj["results"]["Addendum"]["addendumlist"];
    var strAddendum = "";
    for (var i=0; i<arr.length; i++) {
        if (arr[i]["Addendum"]) {
            strAddendum += arr[i]["Addendum"] + " \nby "+arr[i]["AddendumUser"]+" (" + arr[i]["AddendumTime"] + ") \n\n";
        }
    }
    $("#tbAddendum").val(strAddendum);
}

//---------------------------------------------------------------------------------------------
//function setSelect(name,val){
	//$('select[name="'+name+'"]').val(val);
//}

//---------------------------------------------------------------------------------------------
function fillDataDynamicRows(obj){
    //                                           console.log(obj);
	for (var i=0; i<obj.length; i++) {
		try {
			var row = obj[i];
			// e.g. {name: "Other", type: "anesEvalPlanItems", val: "ON", notes: "a'b$c%"@+-", hl: "Y"} 
			var target = row.type+"-"+row.name;
	        //console.log(target + "," + row.val);
                                           
			if (row.hl=="Y"){
				//console.log("#"+target+"-hl");
				// TODO: Make sure all -hl have id
				$("[name="+target+"-hl]").val(row.hl);
			}
		
			if ($("[name="+target+"-ta]").length > 0 && row.notes){
				$("[name="+target+"-ta]").val(replaceBR(row.notes));// replace <br/> with new line
	            //console.log("#"+target+"-ta");
			}
		
			if ($("[name="+target+"-cb]").length>0){
				$("[name="+target+"-cb]").prop("checked",(row.val == "ON")?true:false);
			}
			else if ($("[name="+target+"-ddl]").length>0){
				$("[name="+target+"-ddl]").val(row.val);
			}
			else if ($("[name="+target+"-rb]").length>0){
				//console.log(row.val);
				$("[name="+target+"-rb][value="+row.val+"]").attr('checked', true);
			}
			else {
				if ($("[name="+target+"-rb]").length>0){
					//console.log("ui not found? " + target + " : " + row.val);
				}
			
			}
		}
		catch(err) {
		    console.log("FILL ERROR: " + err.message);
		}		
	}
}

//---------------------------------------------------------------------------------------------
function fillData(obj){
	$("input[type=checkbox]").each(function(index){
	 if (obj[$(this).attr("name")]) {
		 $(this).attr("checked","checked");
	 }
	});
	
	// working? test this
	$("input[type=radio]").each(function(index){
	  var tmpVal = obj[$(this).attr("name")];
	  if (tmpVal && $(this).val() == tmpVal) {
		  $(this).attr("checked", true);
	  }
    });
	
	// add hidden field and same action when you hit done on sin box?
	$("input[type=text],input[type=number],input[type=hidden],select,textarea").each(function(index) {
		var node = obj[$(this).attr("name")];
		 if (node){
			$(this).val(node.toString());
		 }
	 });
}

function hideFacesheet(){
    $(".facesheet_section").hide();
    $(".facesheet_label_section").hide();
}

function refreshPreopSignatures(){
    window.location = "ipro://refreshPreopSignatures/";
}

function refreshPostopSignatures(){
    window.location = "ipro://refreshPostopSignatures/";
}

function setupBridgeSync(protocol_url){
    $(".bridge_sync").change(function(){
        window.location = protocol_url + $(this).attr("rel") + "," + $(this).is(':checked');
    });
}

function bindOpSignature(sigData){
    //{
    //      "ProcID":6557,
    //      "SignedDateTime":"05/03/2013 12:37",
    //      "SignaturePlaceID":"11",
    //      "SignatureType":"performedby"
    //}
    
    // target button id = sig11
    // target text id = sigName11
    
    for (var i=0; i<sigData.length; i++){
        var sigDict = sigData[i];
        $("#sig"+sigDict.SignaturePlaceID).hide();
        $("#sigName"+sigDict.SignaturePlaceID).show();
        $("#sigTime"+sigDict.SignaturePlaceID).show();
        
        var txt = "";
        if (sigDict.SignatureType == "performedby"){
            txt += "Performed by ";
        }
        else if (sigDict.SignatureType == "signedby"){
            txt += "Signed by ";
        }
        else if (sigDict.SignatureType == "both"){
            txt += "Performed and Signed by ";
        }
    
        txt += sigDict.Name;
        $("#sigName"+sigDict.SignaturePlaceID).text(txt);
        
        $("#sigTime"+sigDict.SignaturePlaceID).text(" (" + sigDict.SignedDateTime + ") ");
    }
    
}


//bindPostOpSignature([{"Name":"Kiichi Takeuchi","SignedDateTime":"08/20/2014 12:34","UserID":39},{"Name":"John Doe","SignedDateTime":"08/20/2014 11:12","UserID":12},],12,false)
function bindPostOpSignature(sigData,currentUserId,isCompleted){
	$(".postop_sig_history").remove();
	for (var i=0; i<sigData.length; i++){
        var sigDict = sigData[i];
		
		if (sigDict.UserID == currentUserId && i==sigData.length-1){
        	$("#sig20").hide();
	        $("#sigName20").show();
	        $("#sigTime20").show();
	        $("#sigName20").text("Signed by " + sigDict.Name);        
	        $("#sigTime20").text(" (" + sigDict.SignedDateTime + ") ");			
		}
		else {
			$("#sigTime20").after("<div class='postop_sig_history' style='margin-top:10px;'>Signed by " + sigDict.Name + "  (" + sigDict.SignedDateTime + ")</div>");
		}
	}   
	
	if (isCompleted == true){
		$("#sig20").hide();
	} 
}

//--------------------------------------------------------------------------------------------------
function isEmptyOrNull(val){
    if (val == '<null>' || val == '(null)' || val == ''){
        return true;
    }
    return false;
}
//--------------------------------------------------------------------------------------------------
function bindTimeBySigIndex(index, nameVal, timeVal,isUnlocked){
	var nameStr = nameVal.replace("(null)","").replace("<null>","").trim();
    var timeStr = timeVal.replace("(null)","").replace("<null>","").trim();
	
	
	
	
    $("#sigTime" + index).text(timeStr);
	$("#sigName" + index).text(nameStr);		
	
	if (nameStr != "" && timeStr != ""){		
		$("#sigName" + index).text($("#sigName" + index).text() + " signed at ");
		$("#sig" + index).hide();
	}
	else {
		if (nameStr != "" && timeStr == ""){
			$("#sig" + index).show();
		}
		else {
			$("#sig" + index).hide();
		}
//		console.log("-" + nameStr + "-" + timeStr+"-");
		//$("#sig" + index).show();
	}
	
    // if (timeStr != "") {
//         $("#sigName" + index).text($("#sigName" + index).text() + " signed at ");
//         $("#sig" + index).hide();
//     }
//     if (isUnlocked && timeStr == "") {
//         $("#sigName" + index).hide();
//         return;
//     }
}

//bindSignature('922','12','<null>','<null>','<null>','<null>','<null>','<null>','<null>','<null>','12:34','12:35','<null>','<null>','<null>','<null>','<null>','<null>','<null>','<null>','Lauren Perugini','Gavi Narra','','','','','','','','','12',true)
//bindSignature('12','39','922','<null>','<null>','<null>','<null>','<null>','<null>','<null>','08/15/2014 09:54','08/15/2014 09:54','<null>','<null>','<null>','<null>','<null>','<null>','<null>','<null>','Gavi Narra','Kiichi Takeuchi','Lauren Perugini','','','','','','','','39',true)
// Pacu Specific
function bindSignature(anes1,anes2,anes3,anes4,
                       crna1,crna2,crna3,crna4,
                       srna1,srna2,
                       anesTime1,anesTime2,anesTime3,anesTime4,
                       crnaTime1,crnaTime2,crnaTime3,crnaTime4,
                       srnaTime1,srnaTime2,
                       anesName1,anesName2,anesName3,anesName4,
                       crnaName1,crnaName2,crnaName3,crnaName4,
                       srnaName1,srnaName2,
                       userId, isUnlocked){
    
	
    bindTimeBySigIndex(0,anesName1,anesTime1,isUnlocked);
    bindTimeBySigIndex(1,anesName2,anesTime2,isUnlocked);
    bindTimeBySigIndex(2,anesName3,anesTime3,isUnlocked);
    bindTimeBySigIndex(3,anesName4,anesTime4,isUnlocked);
    
    bindTimeBySigIndex(4,crnaName1,crnaTime1,isUnlocked);
    bindTimeBySigIndex(5,crnaName2,crnaTime2,isUnlocked);
    bindTimeBySigIndex(6,crnaName3,crnaTime3,isUnlocked);
    bindTimeBySigIndex(7,crnaName4,crnaTime4,isUnlocked);
    
    bindTimeBySigIndex(8,srnaName1,srnaTime1,isUnlocked);
    bindTimeBySigIndex(9,srnaName2,srnaTime2,isUnlocked);
    
}


// Clear some fields for template
function clearAfterTemplate(){
	
	// Checklist
	$("#cbTOReadsAloud").prop('checked', false);
	$("#cbTOYes").prop('checked', false);
	$("#cbTONA").prop('checked', false);
	$("#cbTOPhysician").prop('checked', false);
	$("#cbTOTeam").prop('checked', false);
	$("#cbTOPhysician").prop('checked', false);
	$("#cbTOTeam").prop('checked', false);
	$("#cbTOAgreement").prop('checked', false);
	$("#tbTOTeamPresent").val("");
	$("#rbSterileTechFol").prop('checked', false);
	$("#rbSterileTechNot").prop('checked', false);
	$("#rbSterileTechNot").prop('checked', false);
	$("input[name=rbStdSterileTech]").prop('checked', false);
	$("input[name^=tbAnti]").val("");
	$("#tbAnesStartTime").val("");
	$("#tbAnesEndTime").val("");
	$("#tbAlineTime").val("");
    $("#tbAlineTime2").val("");
	$("#tbAlinePerformedBy").val("");
	$("#tbPALineTime").val("");
    $("#tbPALineTime2").val("");    
	$("#tbPALinePerformedBy").val("");
	$("#tbCVPTime").val("");
    $("#tbCVPTime2").val("");
	$("#tbCVPPerformedBy").val("");
	$("#tbRegStartTime").val("");
	$("#tbRegEndTime").val("");
	$("#tbRegPerformedBy").val("");
	$("#tbPerformedby").val("");
	$("input[name^=tbDrugName1Percent]").val("");
	$("input[name^=tbDrugName1Dose]").val("");
	$("input[name^=tbDrugName2Percent]").val("");
	$("input[name^=tbDrugName2Dose]").val("");
	$("input[name^=tbDrugName3Percent]").val("");
	$("input[name^=tbDrugName3Dose]").val("");
	$("input[name^=tbDrugName4Percent]").val("");
	$("input[name^=tbDrugName4Dose]").val("");
	$("input[name^=tbDrugName5Percent]").val("");
	$("input[name^=tbDrugName5Dose]").val("");
	$("input[name^=tbDrugName6Percent]").val("");
	$("input[name^=tbDrugName6Dose]").val("");
	$("#tbAntiTime1").val("");
    $("#tbAntiTime2").val("");
    $("#tbAntiTime3").val("");
    $("#tbAntiTime4").val("");
    $("#tbStpAntiTime1").val("");
    $("#tbStpAntiTime2").val("");
    $("#tbStpAntiTime3").val("");
    $("#tbStpAntiTime4").val("");
    $("#tbRegDoctor").val("");
    $("#cbTOReadsAloudBlock").prop('checked', false);
    $("#cbTOYesBlock").prop('checked', false);
    $("#cbTONABlock").prop('checked', false);
    $("#cbTOPhysicianBlock").prop('checked', false);
    $("#cbTOTeamBlock").prop('checked', false);
    $("#cbRegBlockReq").prop('checked', false);
    
    
    
	// PACU
	$("#cbAntibioticTimelyDelivered").prop('checked', false).prop("disabled", false);
	$("#cbNDMedicalReasons").prop('checked', false).prop("disabled", false);
	$("#cbNDUnspecificReasons").prop('checked', false).prop("disabled", false);
	$("#cbAntibioticNotOrdered").prop('checked', false).prop("disabled", false);
	$("#cbContPreOp").prop('checked', false).prop("disabled", false);
	$("#cbGivenIntraOp").prop('checked', false).prop("disabled", false);
	$("#cbNotApplicable").prop('checked', false).prop("disabled", false);
    $("#cbCVCFoll").prop('checked', false).prop("disabled", false);
    $("#cbCVCNotFollMed").prop('checked', false).prop("disabled", false);
    $("#cbCVCNotFollUnsp").prop('checked', false).prop("disabled", false);
	$("#cbDurationLessThan60").prop('checked', false).prop("disabled", false);
	$("#cbMACAnesthesia").prop('checked', false).prop("disabled", false);
	$("#cbActiveWarmingPreformed").prop('checked', false).prop("disabled", false);
	$("#cbActiveWarmingNotPreformed").prop('checked', false).prop("disabled", false);
	$("#cbActiveWarmingNotPerformedAchieved").prop('checked', false).prop("disabled", false);
    
	
	$("#tbPACUBP").val("");
	$("#tbpacuHR").val("");
	$("#tbPACUSpO2").val("");
	$("#tbPACUT").val("");
	$("#tbPACURR").val("");    
    $("#tbReversalTime1").val("");
    $("#tbReversalTime2").val("");
    $("#tbReversalTime3").val("");
    $("#tbReversalTime1").val("");
    $("#tbReversalTime2").val("");
    $("#tbReversalTime3").val("");
	$("#tbRevTime1").val("");
	$("#tbRevTime2").val("");
	$("#tbRevTime3").val("");
	$("#tbReversalDose1").val("");
	$("#tbReversalDose2").val("");
	$("#tbReversalDose3").val("");
	//$("#tbRevDrug3").val("");
    $("#tbPACUIN").val("");
    
    $("#cbParticipatingInduction").prop('checked', false);
    $("#cbParticipatingEmergence").prop('checked', false);
    $("#cbParticipatingKeyEvents").prop('checked', false);
	$("#ddlAnesParticipate").val("");
    $("#ddlAnesPartEmerg").val("");
    $("#ddlAnesPartKeyEvents").val("");
    $("#tbAnesParticipateTime").val("");
    $("#tbAnesPartEmergTime").val("");
    $("#tbAnesPartKeyEventTime").val("");
}

function bindPostOp(isCompleted){
    if (isCompleted){
        $("#complete_postop").text("Complete PostOp");
    }
    else {
        $("#complete_postop").text("Unlock PostOp");
    }
}

function competePostOp(){
    var msg = "";
    if ($("#sig20").is(":visible")) {
        msg +=  "- Please sign this document\n";
    }
    if ($("#tbPostOpDoneTime").val() == "") {
        msg +=  "- Please enter Post-op complete time\n";
    }
    if (msg != "") {
        alert(msg);
        return;
    }
    window.location = "ipro://completePostOp/";
}

function openTimeSlot(elementName) {
    openTimeSlot(elementName,"");
}

function openTimeSlot(elementName,type) {
    var scrollTop = $(window).scrollTop();
    var elemWidth=$("#"+elementName).width();
    var elemHeight=$("#"+elementName).height();
    
    var left=$("#"+elementName).position().left;
    var top=$("#"+elementName).position().top;
	var val=$("#"+elementName).val();
    window.location = "ipro://openDatePicker/"+elementName+","+(left+(elemWidth/2))+","+(top-scrollTop+elemHeight)+","+type+","+val;
}

function openDrugView(elementName) {
    var scrollTop = $(window).scrollTop();
    var elemWidth=$("#"+elementName).width();
    var elemHeight=$("#"+elementName).height();
    
    var left=$("#"+elementName).position().left;
    var top=$("#"+elementName).position().top-30;
	var val=$("#"+elementName).val();
    window.location = "ipro://openDrugPicker/"+elementName+","+(left+(elemWidth/2))+","+(top-scrollTop+elemHeight)+","+val;
}

function openPersonnel(elementName,targetElem,type) {
    var scrollTop = $(window).scrollTop();
    var elemWidth=$("#"+elementName).width();
    var elemHeight=$("#"+elementName).height();
    
    var left=$("#"+elementName).position().left;
    var top=$("#"+elementName).position().top;
	var val=$("#"+elementName).val();
    window.location = "ipro://openPersonnel/"+targetElem+","+(left+(elemWidth/2))+","+(top-scrollTop+elemHeight)+","+type+","+val;
}
                                               
function setValue(elementName,value){
	if(value){
		value=replaceBR(value);
	}
    $("#"+elementName).val(value);
}

function appendValue(elementName,value){
	if (value){
		value=replaceBR(value);
	}
    var originalVal = $("#"+elementName).val();//original val
    if (originalVal != ""){
        originalVal += ", ";
    }
    originalVal += value;
    $("#"+elementName).val(originalVal);
}

function setCheckbox(elemId, checked){
    if (checked) {
        $('#' + elemId).prop('checked', true);
    }
    else {
        $('#' + elemId).prop('checked', false);
    }
}

function replaceBR(val){
	if (val){
		return val.replace(/<br\s*[\/]?>/gi, "\n")
	}
	return val;	
}

function replaceNL(val){
	if (val){
		return val.replace(/\n/gi,"<br/>");
	}
	return val;
}

function initMultiSelect(multiSelectId, hiddenInputId){
	$("#"+multiSelectId).change(function(){
		var arr = $("#"+multiSelectId).val();
		var csv = "";
		if (arr && arr.length>0){
			csv = arr.join("|");
		}
		$("#"+hiddenInputId).val(csv);
	});
}

function bindMultiSelect(multiSelectId, hiddenInputId){
	var csv = $("#"+hiddenInputId).val();
	if (csv != null && csv != ""){
		var arr = csv.split("|");
		$("#" + multiSelectId).val(arr);
	}
}

//--------------------------------------------------------------------------------------------------
// not used at this point, make sure no special chars in id name
function escapeSelector(idname){
    return idname.replace(/\./g, "\\\\.").replace(/\[/g,"\\\\[").replace(/\]/g,"\\\\]");
}

//--------------------------------------------------------------------------------------------------
// Return error message if validation failed
function validateCheckListData(){
	var errorMet = 0;
	var errorMsg = "";
	
	
	if (!$('input[name="cbPatientEvaluatedPriorToInduction"]').is(":checked")) {
		errorMsg += "- Incomplete Patient Re-Evaluated Prior to Induction\n";
	}
    
    var nerveBlockSelected = false;
    if ($('select[name="ddlRegional2"]')!=null && $('select[name="ddlRegional2"]').val()!=null && $('select[name="ddlRegional2"]').val().length != 0) {
        nerveBlockSelected = true;
    }
	
//	if ($('select[name="ddlPALine"]').val().length != 0 && !$('input[name="cbTOReadsAloud"]').is(":checked")) {
//		errorMet++;
//	}
//	if ($('select[name="ddlCVP"]').val().length != 0 && !$('input[name="cbTOReadsAloud"]').is(":checked")) {
//		errorMet++;
//	}
	if ($('input[name="cbEpidural"]').is(":checked") && !$('input[name="cbTOReadsAloud"]').is(":checked")) {
		errorMet++;
	}
	if ($('input[name="cbSpinal"]').is(":checked") && !$('input[name="cbTOReadsAloud"]').is(":checked")) {
		errorMet++;
	}
    

    // This is separated validation - clean up later
	if (nerveBlockSelected && !$('input[name="cbTOReadsAloudBlock"]').is(":checked")) {
        errorMsg +=  "- Incomplete Block \"Time-Out\" Documentation\n";
	}
    
    
    var timeOutError = false;
	if(errorMet > 0){
		errorMsg +=  "- Incomplete \"Time-Out\" Documentation\n";
        timeOutError = true;
	}
	
	var sterileChecked = true;
	var sterileCheckedBottom=true;
	
	if ($("#rbSterileTechFol").attr("checked") != "checked" && $("#rbSterileTechNot").attr("checked") != "checked"){
		sterileChecked = false;
	}
	
	//??
	if ($("#rbStdSterileTech").attr("checked") != "checked" && $("#rbStdSterileTechNot").attr("checked") != "checked"){
		sterileCheckedBottom = false;
	}
	
	errorMet = 0;
	
	
    //    if ($('select[name="ddlALineVal"]').val().length != 0 && !sterileChecked) {
    //			errorMet++;
    //    }
	//if ($('select[name="ddlPALine"]').val().length != 0 && !sterileChecked) {
	//	errorMet++;
	//}
	//if ($('select[name="ddlCVP"]').val().length != 0 && !sterileChecked) {
	//	errorMet++;
	//}
    errorMet += checkPALine(); // impleented in checklist.html
    errorMet += checkCVP(); // impleented in checklist.html
	if ($('input[name="cbEpidural"]').is(":checked") && !sterileChecked) {
		errorMet++;
	}
	if ($('input[name="cbSpinal"]').is(":checked") && !sterileChecked) {
		errorMet++;
	}

    
    // This is separated validation
    if (nerveBlockSelected && !sterileCheckedBottom) {
        errorMsg += "- Incomplete Block Sterile Techniques Documentation\n";
	}
	
    // If timeout got error, but serile is checked without any errors,
    // truncate error message
    // because the user needs to fill sterline instead of timeout
    //    if (timeOutError == true && sterileChecked && errorMet == 0) {
    //        errorMsg = "";
    //    }
    //errorMsg += timeOutError + "," + sterileCheckedBottom + "," + errorMet;
	if (errorMet>0){
       errorMsg += "- Incomplete Sterile Techniques Documentation\n";
	}

    
    errorMet = 0;
    if (!$("#cbGeneral").attr("checked") &&
        !$("#cbRegional").attr("checked") &&
        !$("#cbMAC").attr("checked") &&
        !$("#cbInhalation").attr("checked") &&
        !$("#cbTIVA").attr("checked") &&
        !$("#cbLocal").attr("checked")
        ) {
        errorMsg += "- Incomplete Anesthesia Techniques Documentation\n";
    }
	
	return errorMsg;
}


//--------------------------------------------------------------------------------------------------
// Checklist Validation - Return the number of error detected
function checkPALine(){
    var errorCount = 0;
	if ($('select[name="ddlPALine"]').val().length > 1) {
			if ($('#cbTOReadsAloud').is(':checked') || $('#cbTOYes').is(':checked') || $('#cbTONA').is(':checked') ||
				$('#cbTOPhysician').is(':checked') || $('#cbTOTeam').is(':checked') || $('#cbTOAgreement').is(':checked') || $('#cbinsituPACath').is(':checked')) {
				//ok
				unColorTO();
			}else{
				changeColorTO();
                errorCount++;
			}

			if ($('#rbSterileTechFol').is(':checked') || $('#rbSterileTechNot').is(':checked') || $('#cbinsituPACath').is(':checked') ){
				//unColor();
				unColor();
			}else{
				changeColor();
                errorCount++;
			}
	}else{
		if ($('select[name="ddlCVP"]').val().length < 1) {
			unColorTO();
			unColor();
		}
	}
    return errorCount;
}
//--------------------------------------------------------------------------------------------------
// Checklist Validation - Return the number of error detected
// Work around - exitingErrorCount
function checkCVP(exitingErrorCount){
	if (!exitingErrorCount){
		exitingErrorCount = 0;
	}
    var errorCount = 0;
	if ($('select[name="ddlCVP"]').val().length > 1) {
			if ($('#cbTOReadsAloud').is(':checked') || $('#cbTOYes').is(':checked') || $('#cbTONA').is(':checked') ||
				$('#cbTOPhysician').is(':checked') || $('#cbTOTeam').is(':checked') || $('#cbTOAgreement').is(':checked') || $('#cbinsituCVP').is(':checked')) {
				if (exitingErrorCount==0){
					unColorTO();
				}
				
			}else{
				changeColorTO();
                errorCount++;
			}

			if ($('#rbSterileTechFol').is(':checked') || $('#rbSterileTechNot').is(':checked') || $('#cbinsituCVP').is(':checked') ){
				if (exitingErrorCount==0){
					unColor();
				}
			}else{
				changeColor();
                errorCount++;
			}
	}else{
		if ($('select[name="ddlPALine"]').val().length < 1) {
			if (exitingErrorCount==0){
				unColorTO();
				unColor();
			}
		}
	}
    return errorCount;
}


//--------------------------------------------------------------------------------------------------
// Return error message if validation failed
function validatePACUData(){
	var errorMsg = "";
	
	// At least one of those checkboxes must be checked
	if (!$('input[name="cbAntibioticTimelyDelivered"]').is(":checked") &&
		!$('input[name="cbNDMedicalReasons"]').is(":checked")  &&
		!$('input[name="cbNDUnspecificReasons"]').is(":checked")  &&
		!$('input[name="cbAntibioticNotOrdered"]').is(":checked")
		) {
		errorMsg =  "- Incomplete PQRS Documentation\n";// don't appened display only once
	}

	if (!$('input[name="cbDurationLessThan60"]').is(":checked")  &&
		!$('input[name="cbMACAnesthesia"]').is(":checked")  &&
		!$('input[name="cbActiveWarmingPreformed"]').is(":checked")  &&
		!$('input[name="cbActiveWarmingNotPreformed"]').is(":checked")  &&
		!$('input[name="cbActiveWarmingNotPerformedAchieved"]').is(":checked")
		) {
		errorMsg =  "- Incomplete PQRS Documentation\n";// don't appened display only once
	}
	
	// At least one of those checkboxes must be checked
	if (!$('input[name="cbContPreOp"]').is(":checked") &&
		!$('input[name="cbGivenIntraOp"]').is(":checked")  &&
		!$('input[name="cbNotApplicable"]').is(":checked") 
		) {
		errorMsg =  "- Incomplete PQRS Documentation\n";// don't appened display only once
	}
                                                     
     // At least one of those checkboxes must be checked
     if (!$('input[name="cbCVCFoll"]').is(":checked") &&
         !$('input[name="cbCVCNotFollMed"]').is(":checked")  &&
         !$('input[name="cbCVCNotFollUnsp"]').is(":checked")
         ) {
     errorMsg =  "- Incomplete PQRS Documentation\n";// don't appened display only once
     }
	
	
	
	return errorMsg;
}
//--------------------------------------------------------------------------------------------------
function validateOBInitials(){
    var message = "";
    $("#chartTable tr").each(function(){
         var rowNode = $(this);
         rowNode.find(".initials").each(function(){
         if ($(this).val().length == 0) {
            rowNode.find("input").each(function(){
            if ($(this).val().length > 0) {
                 message = "- Please complete Anesthesia Flow Sheet\n";
            }
           });
          }
         });
    });
    return message;
}

//--------------------------------------------------------------------------------------------------
//-------------- DEPEND ON THE ROLE AND NUMBER, BLOCK Certain Things -------------------
// Check signature for signout
function checkSignoutSingature(anes1,anes2,anes3,anes4,crna1,crna2,crna3,crna4,srna1,srna2,userId){
	var errorMsg = "";
       
    if (anes1 == userId && (!isInt(anes1) || $("#sigTime0").text() == "") ){
        errorMsg += "- Please sign as 1st Anesthesiologist\n";
	}
    
    if (anes2 == userId && (!isInt(anes2) || $("#sigTime1").text() == "") ){
        errorMsg += "- Please sign as 2nd Anesthesiologist\n";
	}
    if (anes3 == userId && (!isInt(anes3) || $("#sigTime2").text() == "") ){
        errorMsg += "- Please sign as 3rd Anesthesiologist\n";
	}
    if (anes4 == userId && (!isInt(anes4) || $("#sigTime3").text() == "") ){
        errorMsg += "- Please sign as 4th Anesthesiologist\n";
	} 
    if (crna1 == userId && (!isInt(crna1) || $("#sigTime4").text() == "") ){
        errorMsg += "- Please sign as 1st CRNA\n";
	}
    if (crna2 == userId && (!isInt(crna2) || $("#sigTime5").text() == "") ){
        errorMsg += "- Please sign as 2nd CRNA\n";
	}
    if (crna3 == userId && (!isInt(crna3) || $("#sigTime6").text() == "") ){
        errorMsg += "- Please sign as 3rd CRNA\n";
	}
    if (crna4 == userId && (!isInt(crna4) || $("#sigTime7").text() == "") ){
        errorMsg += "- Please sign as 4th CRNA\n";
	}
    if (srna1 == userId && (!isInt(srna1) || $("#sigTime8").text() == "") ){
        errorMsg += "- Please sign as 1st SRNA\n";
	}
    if (srna2 == userId && (!isInt(srna2) || $("#sigTime9").text() == "") ){
        errorMsg += "- Please sign as 2nd SRNA\n";
	}
    
    return errorMsg;    
}

//--------------------------------------------------------------------------------------------------------
function highlightEpidural(){
  $("#timeOutContainer").css({'background-color':'transparent'});
  $("#sterileTechContainer").css({'background-color':'transparent'});
  
  if ($("#cbEpidural").is(":checked")) {
	  if (!$("#sterileTechContainer input").is(":checked")){
	  	$("#sterileTechContainer").css({'background-color':'#FCF7D9'});
	  }
	  if (!$("#timeOutContainer input").is(":checked")){
	  	$("#timeOutContainer").css({'background-color':'#FCF7D9'});
	  }
  }
}

//--------------------------------------------------------------------------------------------------------
// This function validates and return the summary of error message
// validateOB()
function validateOB(isCSection){
	var errorMsg = "";
    if ($('#tbDeliveryStart').val()==""){
        errorMsg += "- Incomplete Start Time\n";
    }
    if (isCSection == false && $('#tbDeliveryEnd').val()==""){
        errorMsg += "- Incomplete End Time\n";
    }
    if($('#cbEpidural').is(":checked")){
        if(!$("#sterileTechContainer input").is(":checked")){
            errorMsg +=  "- Incomplete Sterile Techniques Documentation\n";
        }
        
        if(!$("#timeOutContainer input").is(":checked")){
            errorMsg +=  "- Incomplete Procedure Time-out Documentation\n";
        }
    }
    // Make sure initials have been filled
    errorMsg += validateOBInitials();	
	return errorMsg;
}

//--------------------------------------------------------------------------------------------------
function isInt(n) {
	if ((""+n).length == 0 || n == ""){
		return false;
	}
	// standaline wise, this does not work anymore
	return typeof n === 'number' || n % 1 == 0;
}

//--------------------------------------------------------------------------------------------------
// For Finalize Button
function checkAllPACUSingatures(anes1,anes2,anes3,anes4,crna1,crna2,crna3,crna4,srna1,srna2,dummyAnesthesiologist){
	
	dummyAnesthesiologist = typeof dummyAnesthesiologist !== 'undefined' ? dummyAnesthesiologist : false;	

	var errorMsg = "";
    
    // No Signature at all
    if (!isInt(anes1) &&
        !isInt(anes2) &&
        !isInt(anes3) &&
        !isInt(anes4) ) {
        errorMsg += "- Please assign Anesthesiologist to finalize the case \n";
    }
    
	if(!dummyAnesthesiologist){
		if (isInt(anes1) &&  $("#sigTime0").text() == ""){
			errorMsg += "- Please sign as 1st Anesthesiologist \n";
		}
	}
    
    if (isInt(anes2) &&  $("#sigTime1").text() == ""){
        errorMsg += "- Please sign as 2nd Anesthesiologist \n";
    }
    if (isInt(anes3) &&  $("#sigTime2").text() == ""){
        errorMsg += "- Please sign as 3nd Anesthesiologist \n";
    }
    if (isInt(anes4) &&  $("#sigTime3").text() == ""){
        errorMsg += "- Please sign as 4th Anesthesiologist \n";
    }    
    if (isInt(crna1) &&  $("#sigTime4").text() == ""){
        errorMsg += "- Please sign as 1st CRNA \n";
    }
    if (isInt(crna2) &&  $("#sigTime5").text() == ""){
        errorMsg += "- Please sign as 2nd CRNA \n";
    }    
    if (isInt(crna3) &&  $("#sigTime6").text() == ""){
        errorMsg += "- Please sign as 3rd CRNA \n";
    }
    if (isInt(crna4) &&  $("#sigTime7").text() == ""){
        errorMsg += "- Please sign as 4th CRNA \n";
    }
    if (isInt(srna1) &&  $("#sigTime8").text() == ""){
        errorMsg += "- Please sign as 1st SRNA \n";
    }
    if (isInt(srna2) &&  $("#sigTime9").text() == ""){
        errorMsg += "- Please sign as 2nd SRNA \n";
    }
    
    return errorMsg;
}

//---------------------------------------------------------------------------------
function checkAllPreOpSignatures(){
    var errorMsg = "";
    // Surgeon Signature at PreOp
    if ($("#sigTime14").text() == ""){
        errorMsg += "- Incomplete PreOp Signature \n";
    }
    return errorMsg;
}

//---------------------------------------------------------------------------------
function checkOBSignature(crna1){
    var errorMsg = "";
    // OB Signature
    if ($("#sigTime30").text() == ""){
        errorMsg += "- Please sign the epidural placement \n";
    }
    // if (isInt(crna1) &&  $("#sigTime31").text() == ""){
    //     errorMsg += "- Please sign as CRNA \n";
    // }
    return errorMsg;
}

//---------------------------------------------------------------------------------
// legendJson sample: [{"ID":1,"Legends":"A - Ropivicaine 0.2"},{"ID":2,"Legends":"B - Ropivacaine 0.5"},...
function bindOBFormDrugLegend(legendJson){
	if (legendJson == null){
		return;
	}
	if (legendJson.length == 0){
		return;
	}
	$("#legend_col_1").html("");
	$("#legend_col_2").html("");
	$("#legend_col_3").html("");
	for (var i=0; i<legendJson.length; i++){
		var col = 1 + parseInt((i/(legendJson.length/3)));
		$("#legend_col_" + col).html($("#legend_col_" + col).html() + "<li>"+legendJson[i].Legends+"</li>");
	}
}

//---------------------------------------------------------------------------------
// legendJson sample: [{"ID":1,"Legends":"A - Ropivicaine 0.2"},{"ID":2,"Legends":"B - Ropivacaine 0.5"},...
function bindOBFormDrugDropdown(legendJson){
	if (legendJson == null){
		return;
	}
	if (legendJson.length == 0){
		return;
	}
	$(".ob_drug_picker").html("<option value=''>-</option>");
	for (var i=0; i<legendJson.length; i++){
		$(".ob_drug_picker").html($(".ob_drug_picker").html() + "<option value='"+legendJson[i].Code+"'>"+legendJson[i].Legends.replace(/ -/,'&nbsp;&nbsp;&nbsp;-')+"</option>");
	}
}

//---------------------------------------------------------------------------------
function changePACUBGColor(){    
    ///////////////////////////////////////////////////////////////////
    // PACU Vital Requirement Fields    
    $('.bb input[type=checkbox]').each(function(i){
            changeBGColor($(this),'.bbty', true);
    });
    $('.bb input[type=checkbox]').click(function(e){
            changeBGColor($(this),'.bbty', false);
    });
     $('.bb input[type=checkbox]').each(function(i){
            changeBGColor($(this),'.cvc', true);
                                        });
     $('.bb input[type=checkbox]').click(function(e){
            changeBGColor($(this),'.cvc', false);
                                         });
    $('.paa input[type=checkbox]').each(function(i){
            changeBGColor($(this),'.paaty', true);
    });
    $('.paa input[type=checkbox]').click(function(e){
            changeBGColor($(this),'.paaty', false);
    });
    $('.ptm input[type=checkbox]').each(function(i){
            changeBGColor($(this),'.ptmty', true);
    });
    $('.ptm input[type=checkbox]').click(function(e){
            changeBGColor($(this),'.ptmty', false);
    });    
    
    changeBGColor($('.bb input[type=checkbox]'),'.bbty', true);    
    changeBGColor($('.bb input[type=checkbox]'),'.bbty', false);
    changeBGColor($('.bb input[type=checkbox]'),'.cvc', true);
    changeBGColor($('.bb input[type=checkbox]'),'.cvc', false);
    changeBGColor($('.paa input[type=checkbox]'),'.paaty', true);
    changeBGColor($('.paa input[type=checkbox]'),'.paaty', false);	
    changeBGColor($('.ptm input[type=checkbox]'),'.ptmty', true);    
    changeBGColor($('.ptm input[type=checkbox]'),'.ptmty', false);
}

//--------------------------------------------------------------------------------------------------
function changeBGColor(element, tdclass, sir){
    $(tdclass +' input[type=checkbox]').each(function(i){
           if($(this).is(':checked')){
           $(tdclass).css({'background-color':'transparent'});
                return sir;
           }
           else{
                $(tdclass).css({'background-color':'#FCF7D9'});
           }
    });
}
//--------------------------------------------------------------------------------------------------
function blinkElement(elemID){
	$("#" + elemID).fadeIn(200).fadeOut(200).fadeIn(200).fadeOut(200).fadeIn(200);
}

//--------------------------------------------------------------------------------------------------
function indicateChange(elemID){
	var originalBg = $("#" + elemID).css("background-color");
    $("#" + elemID).animate({backgroundColor: '#ffcc66'}, 'slow').animate({backgroundColor: originalBg}, 'fast').animate({backgroundColor: '#ffcc66'}, 'slow').animate({backgroundColor: originalBg}, 'fast');
}

//--------------------------------------------------------------------------------------------------
function capitaliseFirstLetter(string){
    return string.charAt(0).toUpperCase() + string.slice(1);
}

//--------------------------------------------------------------------------------------------------
function initSCMAllergies(){
	
	
	$("#Allergies").hide();
    $("#SCMAllergies").tagit({
//        availableTags: defaultTags,
        beforeTagAdded: function(evt, ui) {
            if (!ui.duringInitialization) {

            }
        },
        afterTagAdded: function(evt, ui) {
            if (!ui.duringInitialization) {
            }
        },
        beforeTagRemoved: function(evt, ui) {
			if (confirm('Are you sure to delete this item?')){
				return true;
			}
			return false;
        },
        afterTagRemoved: function(evt, ui) {
        },
        onTagClicked: function(evt, ui) {
			console.log("clicked:"+ ui.tag);
        },
        onTagExists: function(evt, ui) {
        }
    });
}

//--------------------------------------------------------------------------------------------------
function fillTestData(){
    
    // invalid input error
    //var testStr = "t,.\"'#()est's1$%^&-+=@!?/\\><M";
    
    // No angle bracket: An error occurred while updating the entries.
    //var testStr = "t,.\"'#()est's1$%^&-+=@!?/\\M";
    
    // if it's too long some field will get errors
    var testStr = "a'b$c%\"@+-";
    
    //success 
//    var testStr = "test";
    
	var testNum = 123;
    var testtime = "11:22";
    $("input[type=text]").val(testStr);
	//$("input[type=text]").each(function(){
	//	$(this).val($(this).attr("id")); //+ testStr
	//});
    $("input[pattern=\"\[0-9\]*\"]").val(testNum);
    $("input[type=number]").val(testNum);
    $("input[type=time]").val(testtime);
    $("input[type=checkbox]").attr("checked","checked");
	$("input[type=radio]").attr("checked", true);
    
    // Anything contains "Time" in the name
    $("input[name*='Time']").val("12:34");
                                                     
                                                     
    $("textarea").val(testStr);
	//$("textarea").each(function(){
	//	$(this).val($(this).attr("id")); // + testStr
	//});
    $("select option:last-child").attr("selected", "selected");
	$("#ddlRegional").val("Single lumbar/Sacral");
	
	// For New Preop 
	$("input[name$='hl']").val('Y');
	
	$("#ddlRegional2").html('<option value="">Select</option>'+
	                        '<option value="Single lumbar/Sacral" selected>Single lumbar/Sacral</option>'+
	                        '<option value="Cont. Lumbar/Sacral">Cont. Lumbar/Sacral</option>'+
	                        '<option value="Bier block">Bier block</option>'+
	                        '<option value="Femoral block">Femoral block</option>'+
	                        '<option value="Sciatic block">Sciatic block</option>'+
	                        '<option value="Post tibial block">Post tibial block</option>'+
	                        '<option value="Interscalene block">Interscalene block</option>'+
	                        '<option value="supraclavicular block">supraclavicular block</option>'+
	                        '<option value="Intercostal block">Intercostal block</option>'+
	                        '<option value="Thoracic epidural Cont.">Thoracic epidural Cont.</option>'+
	                        '<option value="Single SHT">Single SHT</option>'+
	                        '<option value="Caudal">Caudal</option>'+
	                        '<option value="Right Axillary">Right Axillary</option>'+
	                        '<option value="Left Axillary">Left Axillary</option>'+
	                        '<option value="Other">Other</option>'
	                        );	

}

//--------------------------------------------------------------------------------------------------
function fillTestPatient(){
	// 
}

//--------------------------------------------------------------------------------------------------
function fillTestOBRows(){	
	var testJson = [{"ID":1,"Legends":"A - Ropivicaine 0.2"},{"ID":2,"Legends":"B - Ropivacaine 0.5"},{"ID":3,"Legends":"C - Robivicaine 0.125% + Fentanyl 2 mcg/ml"},{"ID":4,"Legends":"D - Lidocaine 2"},{"ID":5,"Legends":"E - Lidocaine 1.5"},{"ID":6,"Legends":"F - Bupivacaine 0.5"},{"ID":7,"Legends":"G - Bupivacaine 0.125 + Fentanyl 2mcg/m"},{"ID":8,"Legends":"H - Bupivacaine 0.25%"},{"ID":9,"Legends":"I - Other"},{"ID":10,"Legends":"J - Bicarbonate 1:10"},{"ID":11,"Legends":"X - Fentanyl 100 mcg"},{"ID":12,"Legends":"Y - Fentanyl 50 mc"}];
	bindOBFormDrugLegend(testJson);
	bindOBFormDrugDropdown(testJson);
	$("#chart_start").val("12:34");
	$("#chart_end").val("12:56");
	//setDebugMode()
	setCurrentUser("John Doe");
	
	var testRows = [{"end":"00:23","infusion_epi":"4","infusion_rate":"1","hr":"2","bolus_epi":"4","infusion_drug":"6","bolus_drug":"2","initials":"Takeuchi, Kiichi","bolus_volume":"99","start":"00:23","bp":"1"},{"end":"11:29","infusion_epi":"2","infusion_rate":"12","hr":"1","bolus_epi":"2","infusion_drug":"4","bolus_drug":"3","initials":"Takeuchi, Kiichi","bolus_volume":"100","start":"11:29","bp":"2"},{"end":"16:34","infusion_epi":"2","infusion_rate":"21","hr":"2","bolus_epi":"2","infusion_drug":"6","bolus_drug":"5","initials":"Takeuchi, Kiichi","bolus_volume":"22","start":"16:34","bp":"2"},{"end":"17:34","infusion_epi":"4","infusion_rate":"3","hr":"3","bolus_epi":"2","infusion_drug":"6","bolus_drug":"6","initials":"Takeuchi, Kiichi","bolus_volume":"3","start":"17:34","bp":"2"},{"end":"","infusion_epi":"","infusion_rate":"","hr":"","bolus_epi":"","infusion_drug":"","bolus_drug":"","initials":"","bolus_volume":"","start":"","bp":""}];
	bindOBProcedureRows(testRows);
}

//--------------------------------------------------------------------------------------------------
// show benchmark result of function
// e.g. benchMe(showDetails,null,5)
function benchMe(func, args, cnt){
	if (!cnt || cnt <= 0){
		cnt = 1;
	}
	var startTime = (new Date()).getTime();
	for (var i=0; i<cnt; i++){
		func(args);
	}
	return (((new Date()).getTime()-startTime)/cnt);
}


//--------------------------------------------------------------------------------------------------
function setDebugMode(){
    DEBUG_MODE = true;

    // Replace all i18n meta tags
    $("body").html($("body").html().replace(/{#/g,"").replace(/#}/g,""));    
}


//////////////////////////// Move these SMC Functions in proper place after implementations
var testCSMAllergies = {"status":"ok","message":"","results":{"Allergies":[{"AllergyID":59,"ScmAllergyID":"9000002262100060","ProcID":8002,"Allergen":"Lactose","AllergenType":"Food","AllergyCategoryType":"Allergy","AllergyType":"Standard","ConfidenceLevel":"Suspected","ConfirmedBy":"Anand Achaibar","Description":"This is a Test Put from Fiddler by Anand Jan 30","InformationSource":"Nurse","OnsetDate":"2014-10-15T00:00:00","ScmAllergyReactions":[{"AllergyReactionID":67,"AllergyID":59,"Reaction":"Blisters","Severity":"Mild"}],"Reason":null,"AddedOn":"2015-01-30T11:05:04.213"},{"AllergyID":60,"ScmAllergyID":"9000002261300060","ProcID":8002,"Allergen":"Soy","AllergenType":"Food","AllergyCategoryType":"Allergy","AllergyType":"Standard","ConfidenceLevel":"Suspected","ConfirmedBy":"Anand Achaibar","Description":"This is a Test Put from Fiddler by Anand","InformationSource":"Nurse","OnsetDate":"2014-10-15T00:00:00","ScmAllergyReactions":[{"AllergyReactionID":68,"AllergyID":60,"Reaction":"Blisters","Severity":"Mild"}],"Reason":null,"AddedOn":"2015-01-30T11:05:04.277"},{"AllergyID":61,"ScmAllergyID":"9000002262300060","ProcID":8002,"Allergen":"Amino-Cerv","AllergenType":"Drug","AllergyCategoryType":"Allergy","AllergyType":"Standard","ConfidenceLevel":"Suspected","ConfirmedBy":"Anand Achaibar","Description":"This is a Test Post from Fiddler by Anand","InformationSource":"Nurse","OnsetDate":"2014-10-15T00:00:00","ScmAllergyReactions":[{"AllergyReactionID":69,"AllergyID":61,"Reaction":"Anxiety","Severity":"Mild"}],"Reason":null,"AddedOn":"2015-01-30T11:05:04.277"},{"AllergyID":62,"ScmAllergyID":"9000002261700060","ProcID":8002,"Allergen":"Peanut Oil","AllergenType":"Food","AllergyCategoryType":"Allergy","AllergyType":"Standard","ConfidenceLevel":"Confirmed","ConfirmedBy":"test","Description":"A New Put Test By Anand jan 30","InformationSource":"Physician","OnsetDate":"2015-01-01T00:00:00","ScmAllergyReactions":[{"AllergyReactionID":70,"AllergyID":62,"Reaction":"Blisters","Severity":"Mild"},{"AllergyReactionID":71,"AllergyID":62,"Reaction":"Blisters","Severity":"Severe"},{"AllergyReactionID":72,"AllergyID":62,"Reaction":"Blisters","Severity":"Moderate"}],"Reason":"A Reson","AddedOn":"2015-01-29T00:00:00"}],"Visit":{"ScmVisitID":1,"ProcID":8002,"ClientVisitID":"9000001551700270","MedRecNum":"000006873","VisitIdCode":"000010407"}}};
function bindSCMAllergies(containerId,data){
	var html = "";
	html += '<table style="width:100%;border:0px"><tr><td class="heading" colspan="2">Allergies <a id="scm_allergies_button" onclick="openSCMAllergies(\'scm_allergies_button\',[])" class="no_freeze button blue">Add Allergy</a></td><td class="heading" colspan="1"><a id="scm_allergies_refresh" onclick="refreshSCMAllergies()" class="no_freeze button blue">Refresh</a></td></tr>';
	var arr = data["results"]["Allergies"];
	for (var i=0; i<arr.length; i++){
		
		//"ScmAllergyReactions":[{"AllergyReactionID":67,"AllergyID":59,"Reaction":"Blisters","Severity":"Mild"}]

		var reactions = arr[i]["ScmAllergyReactions"];
		var reactionHtml = '';
		for (var j=0; j<reactions.length;j++){
			reactionHtml += reactions[j]["Reaction"];
			if (reactions[j]["Severity"] && reactions[j]["Severity"] != ""){
				reactionHtml += " (" + reactions[j]["Severity"] + ")";
			}
			reactionHtml += "<br/>";
		}
		html += '<tr style="border:0px"><td width="15%"><label for="allergen1">'+arr[i]["Allergen"]+'</label></td><td>'+reactionHtml+'</td><td width="10%"><a id="SCMAllergyID_'+arr[i]["ScmAllergyID"]+'" class="button blue" onclick="openSCMAllergies(\'SCMAllergyID_'+arr[i]["ScmAllergyID"]+'\',\''+arr[i]["ScmAllergyID"]+'\')">Details</a></td></tr>';
	}
	html += "</table>";
	$("#"+containerId).html(html);
}


//---------------------------------------------------------------------------------------------
function refreshSCMAllergies(){
	window.location = "ipro://refreshSCMAllergies/";	    
}

function refreshSCMMeds(){
	window.location = "ipro://refreshSCMMeds/";
}

var testHomeMeds = {"status":"ok","message":"","results":{"SCMHomeMeds":[{"SCMHomeMedID":"2623","ProcID":13530,"Comments":"","CreatedDate":"1/21/2015 11:20:35 AM","DoseAmount":"1","DoseForm":"solution","DoseStrength":"300 mg/mL","DoseUOM":"g","DrugName":"lincomycin 300 mg/mL injectable solution","EndDate":"","EndDateApproximated":"False","ExtractedDrugName":"lincomycin ","FollowUpComment":"","Frequency":"once a day","InformationSource":"","Instructions":"1 g injectable once a day as needed","IsPrn":"True","LastDoseTaken":"","LastDoseTakenDate":null,"LastDoseTakenTime":null,"MultumDNum":"d00279","MultumMMDC":"1917","Reason":null,"Route":"injectable","RxNorm":"239212","StartDate":"1/12/2015 12:00:00 AM","StartDateApproximated":"False","Status":"Active"},{"SCMHomeMedID":"2624","ProcID":13530,"Comments":"","CreatedDate":"1/21/2015 11:24:33 AM","DoseAmount":"1","DoseForm":"solution","DoseStrength":"300 mg/mL","DoseUOM":"g","DrugName":"lincomycin 300 mg/mL injectable solution","EndDate":"","EndDateApproximated":"False","ExtractedDrugName":"lincomycin ","FollowUpComment":"","Frequency":"once a day","InformationSource":"","Instructions":"1 g injectable once a day as needed","IsPrn":"True","LastDoseTaken":"","LastDoseTakenDate":null,"LastDoseTakenTime":null,"MultumDNum":"d00279","MultumMMDC":"1917","Reason":null,"Route":"injectable","RxNorm":"239212","StartDate":"1/12/2015 12:00:00 AM","StartDateApproximated":"False","Status":"Active"},{"SCMHomeMedID":"2625","ProcID":13530,"Comments":"","CreatedDate":"1/21/2015 11:33:11 AM","DoseAmount":"1","DoseForm":"solution","DoseStrength":"300 mg/mL","DoseUOM":"g","DrugName":"lincomycin 300 mg/mL injectable solution","EndDate":"","EndDateApproximated":"False","ExtractedDrugName":"lincomycin ","FollowUpComment":"","Frequency":"once a day","InformationSource":"","Instructions":"1 g injectable once a day as needed","IsPrn":"True","LastDoseTaken":"","LastDoseTakenDate":null,"LastDoseTakenTime":null,"MultumDNum":"d00279","MultumMMDC":"1917","Reason":null,"Route":"injectable","RxNorm":"239212","StartDate":"1/12/2015 12:00:00 AM","StartDateApproximated":"False","Status":"Active"},{"SCMHomeMedID":"2627","ProcID":13530,"Comments":"","CreatedDate":"1/21/2015 12:00:17 PM","DoseAmount":"1","DoseForm":"","DoseStrength":null,"DoseUOM":"dose","DrugName":"lisa drugs","EndDate":"","EndDateApproximated":"False","ExtractedDrugName":"lisa drugs","FollowUpComment":"","Frequency":"4 times a day","InformationSource":"","Instructions":"1 dose inhalation 4 times a day","IsPrn":"False","LastDoseTaken":"","LastDoseTakenDate":null,"LastDoseTakenTime":null,"MultumDNum":null,"MultumMMDC":null,"Reason":null,"Route":"inhalation","RxNorm":null,"StartDate":"","StartDateApproximated":"False","Status":"Active"},{"SCMHomeMedID":"2628","ProcID":13530,"Comments":"This is my Test","CreatedDate":"1/22/2015 10:00:11 AM","DoseAmount":"1","DoseForm":"","DoseStrength":null,"DoseUOM":"mg","DrugName":"Morphine 100 mg oral tablet, extended release","EndDate":"","EndDateApproximated":"False","ExtractedDrugName":"Morphine 100 mg oral tablet, extended release","FollowUpComment":"","Frequency":"","InformationSource":"","Instructions":"1 mg oral After breakfast","IsPrn":"False","LastDoseTaken":"","LastDoseTakenDate":null,"LastDoseTakenTime":null,"MultumDNum":"d00308","MultumMMDC":null,"Reason":null,"Route":"oral","RxNorm":null,"StartDate":"","StartDateApproximated":"False","Status":"Active"}]}};
//bindSCMHomeMeds('home_meds',testHomeMeds);
function bindSCMHomeMeds(containerId,data){
	var html = "";
	html += '<table class="mainTable" style="width:90%;margin-left:15px;"><tbody><tr><td class="normal">';
	html += '<table style="width:100%;border:0px"><tr><td class="heading">Home Medications <a id="scm_homemeds_button" onclick="openSCMHomeMeds(\'scm_homemeds_button\',[])" class="button blue">Add Home Medication</a></td><td class="heading" colspan="1"><a id="scm_homemeds_refresh" onclick="refreshSCMHomeMeds()" class="no_freeze button blue">Refresh</a></td></tr>';
	var arr = data["results"]["SCMHomeMeds"];
	for (var i=0; i<arr.length; i++){				
		html += '<tr style="border:0px"><td><label>'+arr[i]["ExtractedDrugName"]+' '+arr[i]["DoseStrength"]+' '+arr[i]["Instructions"]+'</label></td><td width="5%" align="center"><a id="SCMHomeMedID_'+arr[i]["HomeMedID"]+'" class="button blue" onclick="openHomeMeds(\'HomeMedID_'+arr[i]["HomeMedID"]+'\',\''+arr[i]["HomeMedID"]+'\')">Details</a></td></tr>';
	}
	html += "</table>";
	html += "</td></tr></tbody></table>";
	$("#"+containerId).html(html);
}

var testAdmMeds = {"status":"ok","message":"","results":{"SCMAdministeredMeds":[{"SCMAdministeredMedID":41,"ProcID":8002,"DoseGiven":"10","DrugName":"Acetaminophen 120 mg_ANES","Form":"Tablet","MultumDNum":"d00049","OrderID":"9000113322100680","OrderTaskID":"9000076291103030","Route":"buccal","StartDateTime":"02/04/2015 00:00:00","Uom":"mg"},{"SCMAdministeredMedID":42,"ProcID":8002,"DoseGiven":"300","DrugName":"Acetaminophen 120 mg_ANES","Form":"Tablet","MultumDNum":"d00049","OrderID":"9000113322400680","OrderTaskID":"9000076291403030","Route":"buccal","StartDateTime":"02/04/2015 00:00:00","Uom":"mL"},{"SCMAdministeredMedID":45,"ProcID":8002,"DoseGiven":"389","DrugName":"Acetaminophen 120 mg_ANES","Form":"Tablet","MultumDNum":"d00049","OrderID":"9000113322700680","OrderTaskID":"9000076291703030","Route":"buccal","StartDateTime":"02/04/2015 00:00:00","Uom":"mL"}]}};

function bindSCMAdmMeds(containerId,data){
	var html = "";
	html += '<table class="mainTable" style="width:90%;margin-left:15px;"><tbody><tr><td class="normal">';
	html += '<table style="width:100%;border:0px"><tr><td class="heading">Administrated Medications <a id="scm_admmeds_button" onclick="openSCMAdmMeds(\'AdmMeds\',\'AdmMeds\')" class="button blue">Add Administrated Medication</a></td><td class="heading" colspan="1"><a id="scm_admmeds_refresh" onclick="refreshSCMAdmMeds()" class="no_freeze button blue">Refresh</a></td></tr>';
	var arr = data["results"]["SCMAdministeredMeds"];
	for (var i=0; i<arr.length; i++){				
		html += '<tr style="border:0px"><td><label>'+arr[i]["DrugName"]+' '+arr[i]["DoseGiven"]+' '+arr[i]["Uom"]+' ('+arr[i]["StartDateTime"]+')</label></td><td width="5%" align="center"><a id="SCMAdministeredMedID_'+arr[i]["SCMAdministeredMedID"]+'" class="button blue" onclick="openHomeMeds(\'SCMAdministeredMedID_'+arr[i]["SCMAdministeredMedID"]+'\',\''+arr[i]["SCMAdministeredMedID"]+'\')">Details</a></td></tr>';
	}
	html += "</table>";
	html += "</td></tr></tbody></table>";
	$("#"+containerId).html(html);
}

//----------------------------------------------------------------------------------------------------------
function openGenericPicker(selectorName,elementName,params) {
    var scrollTop = $(window).scrollTop();
    var elemWidth=$("#"+elementName).width();
    var elemHeight=$("#"+elementName).height();
    var left=$("#"+elementName).position().left;
    var top=$("#"+elementName).position().top;
    var val=$("#"+elementName).val();
    var url="ipro://"+selectorName+"/"+elementName+","+(left+(elemWidth/2))+","+(top-scrollTop+elemHeight)+","+params.join(',');
    window.location = url;
}


//----------------------------------------------------------------------------------------------------------
function openSCMAllergies(elementName,SCMAllergyID) {
    openGenericPicker('openSCMAllergies',elementName,[SCMAllergyID]);
}
//----------------------------------------------------------------------------------------------------------
function openSCMHomeMeds(elementName,SCMHomeMedsID) {
    openGenericPicker('openSCMHomeMeds',elementName,[SCMHomeMedsID]);
}

//----------------------------------------------------------------------------------------------------------
function openSCMAdmMeds(elementName,SCMAdmMedsID) {
    openGenericPicker('openSCMAdmMeds',elementName,[SCMAdmMedsID]);
}


/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//---------------------------- MAIN ------------------------------------------
// Startup
$(document).ready(function(){
	// $("input,select,textarea").not(".no_sync").on('change',function(){
	//         if (!DEBUG_MODE){
	//            window.location = "ipro://saveCache/" + $(this).attr("name")+"," + encodeURIComponent($(this).val()) + "," + $(this).is(':checked');
	//         }
	// });
	
	// make sure to trim the chars before .change happens
	$("input[maxlength]").keyup(function(){
	    if ($(this).val() && $(this).val().length > $(this).attr("maxlength")){
	         $(this).val($(this).val().substr(0,$(this).attr("maxlength")));
	    }
	});	
	
    //Label clicking
    $("label").click(function(){});
});


