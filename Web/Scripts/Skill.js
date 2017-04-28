﻿var GetSkill = [{ "Base": "General", "Dep": [{ "name": "General", "Type": [] }] }, { "Base": "Functional Mastery", "Dep": [{ "name": "MKG", "Type": [{ "value": "Basic" }, { "value": "Advance" }] }, { "name": "Packing", "Type": [{ "value": "Basic" }, { "value": "Advance" }] }, { "name": "WH/SNO", "Type": [{ "value": "Basic" }, { "value": "Advance" }] }, { "name": "QA", "Type": [{ "value": "Basic" }, { "value": "Advance" }] }, { "name": "MPD", "Type": [{ "value": "Advance" }] }, { "name": "HSE", "Type": [{ "value": "Advance" }] }, { "name": "Engineering", "Type": [{ "value": "Advance" }] }, { "name": "Plant Wide", "Type": [{ "value": "Advance" }] }] }, { "Base": "IWS Mastery", "Dep": [{ "name": "QA", "Type": [{ "value": "SWP Mastery" }] }] }, { "Base": "IWS TOOLS", "Dep": [{ "name": "IWS ", "Type": [{ "value": "TOOLS" }] }] }];
(function ($) { $.fn.extend({ CreateSkill: function (BackFn) { var html = ""; html += '<tr><td>Type</td><td><select class="form-control" id="Base"  name="Base" >'; html += ' <option value=" ">请选择</option> '; for (var i in GetSkill) { html += ' <option value="' + GetSkill[i].Base + '">' + GetSkill[i].Base + "</option> " } html += " </select></td></tr>  "; html += ' <tr><td>Skill Area</td><td>  <select class="form-control" id="Name"  name="Name" >'; html += ' <option value=" ">请选择</option> '; html += " </select></td></tr> "; if (GetSkill[0].Dep[0].Type.length > 0) { html += ' <tr id="threetr"><td>Skill Blocks</td><td>  <select class="form-control" id="Type"  name="Type" >'; html += ' <option value=" ">请选择</option> '; html += " </select></td></tr>" } $(this).html(html); BackFn(this) }, Skill_Load: function () { var TableJQ = $(this); $(document).on("change", "#Base", function () { var Baseval = $(this).val(); if ($.trim(Baseval) != "") { for (var i in GetSkill) { if (GetSkill[i].Base == Baseval) { var namehtml = ""; for (var o in GetSkill[i].Dep) { namehtml += ' <option value="' + GetSkill[i].Dep[o].name + '">' + GetSkill[i].Dep[o].name + "</option> " } $("#Name").html(namehtml); if (GetSkill[i].Dep[0].Type.length > 0) { TableJQ.find("#threetr").remove(); var threehtml = ""; threehtml += ' <tr id="threetr"><td>Skill Blocks</td><td>  <select class="form-control" id="Type"  name="Type" >'; for (var p in GetSkill[i].Dep[0].Type) { threehtml += ' <option value="' + GetSkill[i].Dep[0].Type[p].value + '">' + GetSkill[i].Dep[0].Type[p].value + "</option> " } threehtml += " </select></td></tr>"; TableJQ.append(threehtml) } else { TableJQ.find("#threetr").remove() } } } } }); $(document).on("change", "#Name", function () { var Baseval = $("#Base").val(); var Nameval = $(this).val(); if ($.trim(Baseval) != "") { for (var i in GetSkill) { if (GetSkill[i].Base == Baseval) { for (var o in GetSkill[i].Dep) { if (GetSkill[i].Dep[o].name == Nameval) { if (GetSkill[i].Dep[o].Type.length > 0) { TableJQ.find("#threetr").remove(); var threehtml = ""; threehtml += ' <tr id="threetr"><td>Skill Blocks</td><td>  <select class="form-control" id="Type"  name="Type" >'; for (var p in GetSkill[i].Dep[o].Type) { threehtml += ' <option value="' + GetSkill[i].Dep[o].Type[p].value + '">' + GetSkill[i].Dep[o].Type[p].value + "</option> " } threehtml += " </select></td></tr>"; TableJQ.append(threehtml) } else { TableJQ.find("#threetr").remove() } } } } } } }) }, BindLoad: function (Base, Name, Type) { if ($.trim(Base) != "") { $("#Base").val(Base); $("#Base").change(); $("#Name").val(Name); $("#Name").change(); $("#Type").val(Type) } } }) }(jQuery));