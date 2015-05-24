/*
 * multiselect2side jQuery plugin
 *
 * Copyright (c) 2010 Giovanni Casassa (senamion.com - senamion.it)
 *
 * Dual licensed under the MIT (MIT-LICENSE.txt)
 * and GPL (GPL-LICENSE.txt) licenses.
 *
 * http://www.senamion.com
 *
 * Revision Note:
 * 2014-4-14: Added a custom event namded 'SelectedChanged' with an argument containing three members: type, OperType and AffectedValues. Which will be fired when any selection or deselection occurs.
 * Warning: There's still a known bug that we have not fixed. Select-All operation can not work normally after Remove-One operation executed once.
 * 2014-4-18: Fixed a stubborn bug which has been confusing me for a while. It occurs once a while after some operations executed especially the Add-all operation. 
 *  When it happens State of the original Select element would be inconsistent with the Selected Select element's. Finally it got fixed after I changed the way 
 *  modifying the selection state for each option element within the original Select element to following this procedure: Firstly, removing the specified option element from the 
 *  original Select element. After then adding it back to the original Select elemenet and now we can successfully perform the modification of its selection state.
 * 2014-5-8£ºChanged the type of 'ChangedValues' which is one of SelectedChanged's args from html option to key-value pair.
 *
 *
 */

(function ($) {
    // SORT INTERNAL
    function internalSort(a, b) {
        var compA = $(a).text().toUpperCase();
        var compB = $(b).text().toUpperCase();
        return (compA < compB) ? -1 : (compA > compB) ? 1 : 0;
    };

    var methods = {
        init: function (options) {
            var o = {
                selectedPosition: 'right',
                moveOptions: true,
                labelTop: 'Top',
                labelBottom: 'Bottom',
                labelUp: 'Up',
                labelDown: 'Down',
                labelSort: 'Sort',
                labelsx: 'Available',
                labeldx: 'Selected',
                maxSelected: -1,
                autoSort: true,
                autoSortAvailable: true,
                search: false,
                caseSensitive: false,
                delay: 200,
                optGroupSearch: false,
                minSize: 7
            };

            return this.each(function () {
                var el = $(this);
                var data = el.data('multiselect2side');

                if (options)
                    $.extend(o, options);

                if (!data)
                    el.data('multiselect2side', o);

                var originalName = $(this).attr("name");
                if (originalName.indexOf('[') != -1)
                    originalName = originalName.substring(0, originalName.indexOf('['));

                var nameDx = originalName + "ms2side__dx";
                var nameSx = originalName + "ms2side__sx";
                var size = $(this).attr("size");
                // SIZE MIN
                if (size < o.minSize) {
                    $(this).attr("size", "" + o.minSize);
                    size = o.minSize;
                }

                // UP AND DOWN
                var divUpDown =
						"<div class='ms2side__updown'>" +
							"<p class='SelSort' title='Sort'>" + o.labelSort + "</p>" +
							"<p class='MoveTop' title='Move on top selected option'>" + o.labelTop + "</p>" +
							"<p class='MoveUp' title='Move up selected option'>" + o.labelUp + "</p>" +
							"<p class='MoveDown' title='Move down selected option'>" + o.labelDown + "</p>" +
							"<p class='MoveBottom' title='Move on bottom selected option'>" + o.labelBottom + "</p>" +
						"</div>";

                // INPUT TEXT FOR SEARCH OPTION
                var leftSearch = false, rightSearch = false;
                // BOTH SEARCH AND OPTGROUP SEARCH
                if (o.search != false && o.optGroupSearch != false) {
                    var ss =
						o.optGroupSearch + "<select class='small' ><option value=__null__> </option></select> " +
						o.search + "<input class='small' type='text' /><a href='#'> </a>";

                    if (o.selectedPosition == 'right')
                        leftSearch = ss;
                    else
                        rightSearch = ss;
                }
                else if (o.search != false) {
                    //Following lines temporarily adding for show of both search textboxes.id='leftSearchTxt' name='leftSearchTxt'id='rightSearchTxt' name='rightSearchTxt'
                    leftSearch = "<input type='text' class='swim-search' placeholder='" + o.search + "' />";
                    rightSearch = "<input type='text' class='swim-search' placeholder='" + o.search + "' />";
                }
                else if (o.optGroupSearch != false) {
                    var ss = o.optGroupSearch + "<select><option value=__null__> </option></select>";

                    if (o.selectedPosition == 'right')
                        leftSearch = ss;
                    else
                        rightSearch = ss;
                }


                // CREATE NEW ELEMENT (AND HIDE IT) AFTER THE HIDDED ORGINAL SELECT
                //********Customized by simon***************
                var htmlToAdd =
					"<div class='ms2side__div'>" +
						"<div class='ms2side__select'>" +
							"<div class='ms2side__header'>" + leftSearch + '<span class="glyphicon glyphicon-search">&nbsp;</span></div>' +
							"<select title='" + o.labelsx + "' name='" + nameSx + "' id='" + nameSx + "' size='" + size + "' multiple='multiple' ></select>" +
						"</div>" +
						"<div class='ms2side__options'>" +
                            "<p class='AddOne' title='Add Selected'>&rsaquo;</p>" +
							"<p class='AddAll' title='Add All'>&raquo;</p>" +
							"<p class='RemoveOne' title='Remove Selected'>&lsaquo;</p>" +
							"<p class='RemoveAll' title='Remove All'>&laquo;</p>" +
						"</div>" +
						"<div class='ms2side__select'>" +
							"<div class='ms2side__header'>" + rightSearch + '<span class="glyphicon glyphicon-search">&nbsp;</span></div>' +
							"<select title='" + o.labeldx + "' name='" + nameDx + "' id='" + nameDx + "' size='" + size + "' multiple='multiple' ></select>" +
						"</div>" + "</div>";

                el.after(htmlToAdd).hide();
                //el.after(htmlToAdd);
                // ELEMENTS
                //**Customized by simon****************
                var allSel = el.next().children(".ms2side__select").children("select");
                var leftSel = allSel.eq(0);
                var rightSel = allSel.eq(1);

                // HEIGHT DIV
                var heightDiv = $(".ms2side__select").eq(0).height();

                // SELECT optgroup
                var searchSelect = $();

                // SEARCH INPUT
                var searchInput = $(this).next().find("input:text");
                //var removeFilter = searchInput.next().hide();
                //**Customized by simon*******************************
                var leftSearchInput = $(this).next().find("input:text").eq(0);
                var rightSearchInput = $(this).next().find("input:text").eq(1);
                //var leftRemoveFilter = leftSearchInput.next().hide();
                //var rightRemoveFilter = rightSearchInput.next().hide();
                var leftSearchV = false;
                var rightSearchV = false;
                var leftToid = false;
                var rightToid = false;
                //***********************************************************
                var toid = false;
                var searchV = false;


                // SELECT optgroup - ADD ALL OPTGROUP AS OPTION
                if (o.optGroupSearch != false) {
                    var lastOptGroupSearch = false;

                    searchSelect = $(this).next().find("select").eq(0);

                    el.children("optgroup").each(function () {
                        if (searchSelect.find("[value='" + $(this).attr("label") + "']").size() == 0)
                            searchSelect.append("<option value='" + $(this).attr("label") + "'>" + $(this).attr("label") + "</option>");
                    });
                    searchSelect.change(function () {
                        var sEl = $(this);

                        if (sEl.val() != lastOptGroupSearch) {

                            // IF EXIST SET SEARCH TEXT TO VOID
                            if (searchInput.val() != "") {
                                clearTimeout(toid);
                                removeFilter.hide();
                                searchInput.val("");//.trigger('keyup');
                                searchV = "";
                                // fto();
                            }

                            setTimeout(function () {
                                if (sEl.val() == "__null__") {
                                    els = el.find("option:not(:selected)");
                                }
                                else
                                    els = el.find("optgroup[label='" + sEl.val() + "']").children("option:not(:selected)");

                                // REMOVE ORIGINAL ELEMENTS AND ADD OPTION OF OPTGROUP SELECTED
                                leftSel.find("option").remove();
                                els.each(function () {
                                    leftSel.append($(this).clone());
                                });
                                lastOptGroupSearch = sEl.val();
                                leftSel.trigger('change');
                            }, 100);
                        }
                    });
                }

                // SEARCH FUNCTION for the left search textbox Customized by simon
                var leftfto = function () {
                    var els = leftSel.children();
                    var toSearch = el.find("option:not(:selected)");

                    // RESET OptGroupSearch
                    lastOptGroupSearch = "__null__";
                    searchSelect.val("__null__");

                    if (leftSearchV == leftSearchInput.val())
                        return;

                    leftSearchInput
						.addClass("wait")
						.removeAttr("style");

                    leftSearchV = leftSearchInput.val();

                    // A LITTLE TIMEOUT TO VIEW WAIT CLASS ON INPUT ON IE
                    setTimeout(function () {
                        leftSel.children().remove();
                        if (leftSearchV == "") {
                            toSearch.clone().appendTo(leftSel).removeAttr("selected");
                            // leftRemoveFilter.hide();
                        }
                        else {
                            toSearch.each(function () {
                                var myText = $(this).text();

                                if (o.caseSensitive)
                                    find = myText.indexOf(leftSearchV);
                                else
                                    find = myText.toUpperCase().indexOf(leftSearchV.toUpperCase());

                                if (find != -1)
                                    $(this).clone().appendTo(leftSel).removeAttr("selected");
                            });

                            if (leftSel.children().length == 0)
                                leftSearchInput.css({ 'border': '1px red solid' });

                            // leftRemoveFilter.show();
                            leftSel.trigger('change');
                        }
                        leftSel.trigger('change');
                        leftSearchInput.removeClass("wait");
                    }, 5);
                };
                // SEARCH FUNCTION for the right search textbox Customized by simon
                var rightfto = function () {
                    var els = rightSel.children();
                    var toSearch = el.find("option:selected").sort(internalSort);
                    // RESET OptGroupSearch
                    lastOptGroupSearch = "__null__";
                    searchSelect.val("__null__");

                    if (rightSearchV == rightSearchInput.val())
                        return;

                    rightSearchInput
						.addClass("wait")
						.removeAttr("style");

                    rightSearchV = rightSearchInput.val();

                    // A LITTLE TIMEOUT TO VIEW WAIT CLASS ON INPUT ON IE
                    setTimeout(function () {
                        rightSel.children().remove();
                        if (rightSearchV == "") {
                            toSearch.clone().appendTo(rightSel);
                            // rightRemoveFilter.hide();
                        }
                        else {
                            toSearch.each(function () {
                                var myText = $(this).text();

                                if (o.caseSensitive)
                                    find = myText.indexOf(rightSearchV);
                                else
                                    find = myText.toUpperCase().indexOf(rightSearchV.toUpperCase());

                                if (find != -1)
                                    $(this).clone().appendTo(rightSel);
                            });

                            if (rightSel.children().length == 0)
                                rightSearchInput.css({ 'border': '1px red solid' });

                            //rightRemoveFilter.show();
                            rightSel.trigger('change');
                        }
                        rightSel.trigger('change');
                        rightSearchInput.removeClass("wait");
                    }, 5);
                };


                //**Customized by simon
                // REMOVE FILTER ON left SEARCH box
                //leftRemoveFilter.click(function () {
                //    clearTimeout(leftToid);
                //    leftSearchInput.val("");
                //    leftfto();
                //    return false;
                //});
                //// REMOVE FILTER ON right SEARCH box
                //rightRemoveFilter.click(function () {
                //    clearTimeout(rightToid);
                //    rightSearchInput.val("");
                //    rightfto();
                //    return false;
                //});
                // ON CHANGE TEXT INPUT on left search box.
                leftSearchInput.bind(
                     'keyup search input paste cut',
                     function () {
                         clearTimeout(leftToid);
                         leftToid = setTimeout(leftfto, o.delay);
                     }
                );

                // ON CHANGE TEXT INPUT on right search box.
                rightSearchInput.bind(
                     'keyup search input paste cut',
                     function () {
                         clearTimeout(rightToid);
                         rightToid = setTimeout(rightfto, o.delay);
                     }
                );
                //*********************************

                // CENTER MOVE OPTIONS AND UPDOWN OPTIONS
                $(this).next().find('.ms2side__options, .ms2side__updown').each(function () {
                    //var top = ((heightDiv / 2) - ($(this).height() / 2));
                    var top = 0;
                    if (top > 0)
                        $(this).css('padding-top', top + 'px');
                })

                // MOVE SELECTED OPTION TO RIGHT, NOT SELECTED TO LEFT
                $(this).find("option:selected").clone().sort(internalSort).appendTo(rightSel); // .removeAttr("selected");
                $(this).find("option:not(:selected)").clone().appendTo(leftSel);

                // SELECT FIRST LEFT ITEM AND DESELECT IN RIGHT (NOT IN IE6)
                //if (!($.browser.msie && $.browser.version == '6.0')) {
                leftSel.find("option").eq(0).attr("selected", true);
                rightSel.children().removeAttr("selected");
                //}

                // **Note: I removed following lines responsible for automatic sorting of 
                //         both available list and selected list in order to reduce the complexity of this widget.
                //***************************************************************************************************************************************************************

                //// ON CHANGE SORT SELECTED OPTIONS
                //var	nLastAutosort = 0;
                //if (o.autoSort)
                //    rightSel.change(function () {
                //		//var	selectDx = rightSel.find("option");
                //        var selectDx = el.find("option:selected");
                //		if (selectDx.length != nLastAutosort) {
                //			// SORT SELECTED ELEMENT
                //			selectDx.sort(internalSort);
                //			// FIRST REMOVE FROM ORIGINAL SELECT
                //			el.find("option:selected").remove();
                //			// AFTER ADD ON ORIGINAL AND RIGHT SELECT
                //			selectDx.each(function() {
                //				rightSel.append($(this).clone());
                //				$(this).appendTo(el).attr("selected", true);
                //				//el.append($(this).attr("selected", true));		HACK IE6
                //			});
                //			nLastAutosort = selectDx.length;
                //		}
                //	});

                //// ON CHANGE SORT AVAILABLE OPTIONS (NOT NECESSARY IN ORIGINAL SELECT)
                //var	nLastAutosortAvailable = 0;
                //if (o.autoSortAvailable)
                //	leftSel.change(function() {
                //		var	selectSx = leftSel.find("option");

                //		if (selectSx.length != nLastAutosortAvailable) {
                //			// SORT SELECTED ELEMENT
                //			selectSx.sort(internalSort);
                //			// REMOVE ORIGINAL ELEMENTS AND ADD SORTED
                //			leftSel.find("option").remove();
                //			selectSx.each(function() {
                //				leftSel.append($(this).clone());
                //			});
                //			nLastAutosortAvailable = selectSx.length;
                //		}
                //	});

                //*******************************************************************************************************************************
                // Sort selcted items by alpha in ascendant order.
                var sortSelectedList = function () {
                    var selectedItems = el.find("option:selected").clone().sort(internalSort);
                    rightSel.children.remove();
                    rightSel.append(selectedItems);
                };

                var FireSelectedChangeEvent = function SyncPropertyTab(OperationType, InvolvedValues) {
                    el.trigger({ type: "SelectedChanged", OpType: OperationType, ChangedValues: InvolvedValues });
                };

                var ConvertOptionToKeyValuePair = function (affectedOption) {
                    return { OptionText: affectedOption.text(), OptionValue: affectedOption.val() };
                };

                // ON CHANGE REFRESH ALL BUTTON STATUS
                allSel.change(function () {
                    // HACK FOR IE6 (SHOW AND HIDE ORIGINAL SELECT)
                    //if ($.browser.msie && $.browser.version == '6.0')
                    //    el.show().hide();
                    var div = $(this).parent().parent();
                    var selectSx = leftSel.children();
                    var selectDx = rightSel.children();
                    var selectedSx = leftSel.find("option:selected");
                    var selectedDx = rightSel.find("option:selected");

                    if (selectedSx.size() == 0 ||
							(o.maxSelected >= 0 && (selectedSx.size() + selectDx.size()) > o.maxSelected))
                        div.find(".AddOne").addClass('ms2side__hide');
                    else
                        div.find(".AddOne").removeClass('ms2side__hide');

                    // FIRST HIDE ALL
                    div.find(".RemoveOne, .MoveUp, .MoveDown, .MoveTop, .MoveBottom, .SelSort").addClass('ms2side__hide');
                    if (selectDx.size() > 1)
                        div.find(".SelSort").removeClass('ms2side__hide');
                    if (selectedDx.size() > 0) {
                        div.find(".RemoveOne").removeClass('ms2side__hide');
                        // ALL SELECTED - NO MOVE
                        if (selectedDx.size() < selectDx.size()) {	// FOR NOW (JOE) && selectedDx.size() == 1
                            if (selectedDx.val() != selectDx.val())	// FIRST OPTION, NO UP AND TOP BUTTON
                                div.find(".MoveUp, .MoveTop").removeClass('ms2side__hide');
                            if (selectedDx.last().val() != selectDx.last().val())	// LAST OPTION, NO DOWN AND BOTTOM BUTTON
                                div.find(".MoveDown, .MoveBottom").removeClass('ms2side__hide');
                        }
                    }

                    if (selectSx.size() == 0 ||
							(o.maxSelected >= 0 && selectSx.size() >= o.maxSelected))
                        div.find(".AddAll").addClass('ms2side__hide');
                    else
                        div.find(".AddAll").removeClass('ms2side__hide');

                    if (selectDx.size() == 0)
                        div.find(".RemoveAll").addClass('ms2side__hide');
                    else
                        div.find(".RemoveAll").removeClass('ms2side__hide');

                });

                // DOUBLE CLICK ON LEFT SELECT OPTION
                leftSel.dblclick(function () {
                    var affectedValues = new Array();
                    $(this).find("option:selected").each(function (i, selected) {

                        if (o.maxSelected < 0 || rightSel.children().size() < o.maxSelected) {
                            $(this).remove().appendTo(rightSel).attr("selected", false);
                            var affectedOption = el.find("[value='" + $(selected).val() + "']");
                            affectedOption.remove().appendTo(el).attr("selected", true);
                            affectedValues.push(ConvertOptionToKeyValuePair(affectedOption));
                        }
                    });
                    $(this).trigger('change');
                    //Fire onchange event to notify outside observers that selected list has been changed.
                    FireSelectedChangeEvent('add', affectedValues);
                });

                // DOUBLE CLICK ON RIGHT SELECT OPTION
                rightSel.dblclick(function () {
                    var affectedValues = new Array();
                    $(this).find("option:selected").each(function (i, selected) {
                        $(this).remove().appendTo(leftSel).attr("selected", false);
                        var affectedOption = el.find("[value='" + $(selected).val() + "']");
                        affectedOption.remove().appendTo(el).attr("selected", false);
                        affectedValues.push(ConvertOptionToKeyValuePair(affectedOption));
                    });
                    $(this).trigger('change');
                    //Fire onchange event to notify outside observers that selected list has been changed.
                    FireSelectedChangeEvent('remove', affectedValues);
                    // TRIGGER CHANGE AND VALUE NULL FORM OPTGROUP SEARCH (IF EXIST)
                    searchSelect.val("__null__").trigger("change");
                    // TRIGGER CLICK ON REMOVE FILTER (IF EXIST)
                    //removeFilter.click();
                });

                // CLICK ON OPTION
                $(this).next().find('.ms2side__options').children().click(function () {
                    if (!$(this).hasClass("ms2side__hide")) {
                        var affectedValues = new Array();
                        var opType = "add";
                        if ($(this).hasClass("AddOne")) {
                            leftSel.find("option:selected").each(function (i, selected) {
                                $(this).remove().appendTo(rightSel).attr("selected", false);
                                var affectedOption = el.find("[value='" + $(selected).val() + "']");
                                affectedOption.remove().appendTo(el).attr("selected", true);
                                affectedValues.push(ConvertOptionToKeyValuePair(affectedOption));
                            });
                        }
                        else if ($(this).hasClass("AddAll")) {	// ALL SELECTED
                            leftSel.children().each(function (i, child) {
                                $(this).remove().appendTo(rightSel).attr("selected", false);
                                var affectedOption = el.find("[value='" + $(this).val() + "']");
                                affectedOption.remove().appendTo(el).attr("selected", true);
                                affectedValues.push(ConvertOptionToKeyValuePair(affectedOption));
                            });

                        }
                        else if ($(this).hasClass("RemoveOne")) {
                            opType = "remove";
                            rightSel.find("option:selected").each(function (i, selected) {
                                $(this).remove().appendTo(leftSel).attr("selected", false);
                                var affectedOption = el.find("[value='" + $(selected).val() + "']");
                                affectedOption.remove().appendTo(el).attr("selected", false);
                                affectedValues.push(ConvertOptionToKeyValuePair(affectedOption));

                            });
                            // TRIGGER CLICK ON REMOVE FILTER (IF EXIST)
                            // leftRemoveFilter.click();
                            //rightRemoveFilter.click();
                            // TRIGGER CHANGE AND VALUE NULL FORM OPTGROUP SEARCH (IF EXIST)
                            //searchSelect.val("__null__").trigger("change");
                        }
                        else if ($(this).hasClass("RemoveAll")) {	// ALL REMOVED
                            opType = "remove";
                            // since there's a unknown error occured after removing all that is we can not selected any item any more after executing the original logic of removing method.
                            rightSel.children().each(function (i, selected) {
                                $(this).remove().appendTo(leftSel).attr("selected", false);
                                var affectedOption = el.find("[value='" + $(this).val() + "']");
                                affectedOption.remove().appendTo(el).attr("selected", false);
                                affectedValues.push(ConvertOptionToKeyValuePair(affectedOption));
                            });

                            // TRIGGER CLICK ON REMOVE FILTER (IF EXIST)
                            // leftRemoveFilter.click();
                            // rightRemoveFilter.click();
                            // TRIGGER CHANGE AND VALUE NULL FORM OPTGROUP SEARCH (IF EXIST)
                            //searchSelect.val("__null__").trigger("change");
                        }
                        //Fire onchange event to notify outside observers that selected list has been changed.
                        FireSelectedChangeEvent(opType, affectedValues);
                    }

                    leftSel.trigger('change');
                });

                // CLICK ON UP - DOWN
                $(this).next().find('.ms2side__updown').children().click(function () {
                    var selectedDx = rightSel.find("option:selected");
                    var selectDx = rightSel.find("option");

                    if (!$(this).hasClass("ms2side__hide")) {
                        if ($(this).hasClass("SelSort")) {
                            // SORT SELECTED ELEMENT
                            selectDx.sort(internalSort);
                            // FIRST REMOVE FROM ORIGINAL SELECT
                            el.find("option:selected").remove();
                            // AFTER ADD ON ORIGINAL AND RIGHT SELECT
                            selectDx.each(function () {
                                rightSel.append($(this).clone().attr("selected", true));
                                el.append($(this).attr("selected", true));
                            });
                        }
                        else if ($(this).hasClass("MoveUp")) {
                            var prev = selectedDx.first().prev();
                            var hPrev = el.find("[value='" + prev.val() + "']");

                            selectedDx.each(function () {
                                $(this).insertBefore(prev);
                                el.find("[value='" + $(this).val() + "']").insertBefore(hPrev);	// HIDDEN SELECT
                            });
                        }
                        else if ($(this).hasClass("MoveDown")) {
                            var next = selectedDx.last().next();
                            var hNext = el.find("[value='" + next.val() + "']");

                            selectedDx.each(function () {
                                $(this).insertAfter(next);
                                el.find("[value='" + $(this).val() + "']").insertAfter(hNext);	// HIDDEN SELECT
                            });
                        }
                        else if ($(this).hasClass("MoveTop")) {
                            var first = selectDx.first();
                            var hFirst = el.find("[value='" + first.val() + "']");

                            selectedDx.each(function () {
                                $(this).insertBefore(first);
                                el.find("[value='" + $(this).val() + "']").insertBefore(hFirst);	// HIDDEN SELECT
                            });
                        }
                        else if ($(this).hasClass("MoveBottom")) {
                            var last = selectDx.last();
                            var hLast = el.find("[value='" + last.val() + "']");

                            selectedDx.each(function () {
                                last = $(this).insertAfter(last);	// WITH last = SAME POSITION OF SELECTED OPTION AFTER MOVE
                                hLast = el.find("[value='" + $(this).val() + "']").insertAfter(hLast);	// HIDDEN SELECT
                            });
                        }
                    }

                    leftSel.trigger('change');
                });

                // HOVER ON OPTION
                $(this).next().find('.ms2side__options, .ms2side__updown').children().hover(
					function () {
					    $(this).addClass('ms2side_hover');
					},
					function () {
					    $(this).removeClass('ms2side_hover');
					}
				);

                // UPDATE BUTTON ON START
                leftSel.trigger('change');
                // SHOW WHEN ALL READY
                $(this).next().show();
            });
        },
        destroy: function () {
            return this.each(function () {
                var el = $(this);
                var data = el.data('multiselect2side');

                if (!data)
                    return;

                el.show().next().remove();
            });
        },
        addOption: function (options) {
            var oAddOption = {
                name: false,
                value: false,
                selected: false
            };

            return this.each(function () {
                var el = $(this);
                var data = el.data('multiselect2side');

                if (!data)
                    return;

                if (options)
                    $.extend(oAddOption, options);

                var strEl = "<option value='" + oAddOption.value + "' " + (oAddOption.selected ? "selected" : "") + " >" + oAddOption.name + "</option>";

                el.append(strEl);

                // ELEMENTS
                var allSel = el.next().children(".ms2side__select").children("select");
                var leftSel = (data.selectedPosition == 'right') ? allSel.eq(0) : allSel.eq(1);
                var rightSel = (data.selectedPosition == 'right') ? allSel.eq(1) : allSel.eq(0);

                if (oAddOption.selected)
                    rightSel.append(strEl).trigger('change');
                else
                    leftSel.append(strEl).trigger('change');
            });
        }
    };

    $.fn.multiselect2side = function (method) {
        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on jQuery.multiselect2side');
        }
    };

})(jQuery);