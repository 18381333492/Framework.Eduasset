/**
 * multiple select plugin
 * @date: 2017/09/13
 * @author: liugl@inspur.com
 */
function multselInit(){
	$_sel=$("div.multsel");
	$_sel.each(function(){
		$(this).append("<i></i>");
	});
	$_sel.find("span.view").off("click").on("click",function(){
		$_t=$(this).parent("div.multsel");
		if($_t.hasClass('expand')){
			$_t.find(".selist").hide();
			$_t.find("i:first").removeClass("pop");
			$_t.removeClass('expand');
		}else {
			$_t.find(".selist").show();
			$_t.find("i:first").addClass("pop");
			$_t.addClass('expand');
		}
	});
	$_sel.find(".selist").find("a.seitem").off("click").on("click",function(){
		$_this=$(this);
		
		if($_this.hasClass('checked')){
			$_this.removeClass('checked');
		}else{
			$_this.addClass('checked');
		}
		$_sel.find("span.view").click();

		multselCheck($_this.parent(".selist"));

	});
	
	//$(document).off("click").on("click",function(e){
	//	var t = $("div.multsel")[0],target = e.target;
	//	if (t !== target && !$.contains(t, target)) {
	//		$_sel.removeClass('expand').find(".selist").hide();
	//		$_sel.find("i:first").removeClass("pop");
	//	}
	//});
}
function multselCheck($obj){
	var names='',vals=[];
	$obj.find('a.checked').each(function(){
		var $t=$(this);
		names+=$t.html()+'  ';
		vals.push($t.attr("value"));
	});
	multselVals(vals, names);
	if(!names){
		names='请选择...';
	}
	$obj.parent('div.multsel').find('span.view').html(names);		

}
/**
 * interface
 * @param vals
 */
function multselVals(vals, names) {
    $('#CopyToIds').val(vals.join());
    $('#CopyToNames').val(names);
}