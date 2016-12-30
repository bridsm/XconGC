String.prototype.toNumer = function () {
    const text = this.valueOf();
    const result = parseFloat(text.replace(/,/g, ""));
    return isNaN(result) ? 0 : result;
}

var txtQtyChanged = (e) => {

    const $txtQty = $(e.target);

    // check is empty string
    if ($.trim($txtQty.val()) === '') {
        $(e.target).val('0');
        return;
    }

    // check is NaN            
    const qty = $txtQty.val().toNumer();
    if (qty === 0) {
        $(e.target).val('0');
        return;
    }

    const row = $txtQty.closest("tr");
    const price = $(".price", row).text().toNumer();
    const total = price * qty;

    $("#lblTotal", row).text(total);

    var grandTotal = 0.00;
    var tax = 0.00;
    var grandTotalTax = 0.00;

    $("[id=lblTotal]").each((idx, elm) => {
        const _total = $(elm).text().toNumer();
        if (!isNaN(_total)) {
            grandTotal = grandTotal + _total;
            tax = grandTotal * (7 / 100);
            grandTotalTax = grandTotal + tax;
        }
    });

    $("#lblGrandTotal").text(grandTotal.toFixed(2).toString());
    $("#lblTax").text(tax.toFixed(2).toString());
    $("#lblGrandTotalTax").text(grandTotalTax.toFixed(2).toString());
};

var lnkSelectClick = (e) => {
    e.preventDefault();
    const row = $(e.target).closest("tr");
    const code = $(".itemCode", row).text();
    const name = $(".itemName", row).text();
    const size = $(".itemSize", row).text();
    const color = $(".itemColor", row).text();

    const $panel = $("[id*=panelDetailItem]");
    $("[id*=lblItemID]", $panel).text(code);
    $("[id*=lblItemName]", $panel).text(name);

    const $txtSize = $("[id*=txtSize]", $panel);
    const $txtColor = $("[id*=txtColor]", $panel);

    if (size === 'Y') {
        $txtSize.attr('disabled', 'disabled');
    } else {
        $txtSize.removeAttr('disabled');
    }
    if (color === 'Y') {
        $txtColor.attr('disabled', 'disabled');
    } else {
        $txtColor.removeAttr('disabled');
    }

    // assign current id
    $("#hidItemCode").val(code);

    // open dialog
    $("#btnShowPanel").click();
};

var btnCancelClick = (e) => {
    e.preventDefault();
    $("[id*=pop_]").val("");
    $(":file").removeAttr("style").val("");
};

var btnUpdateClick = (e) => {
    e.preventDefault();

    const $panel = $("#formDetailItem");
    const code = $("[id*=lblItemID]", $panel).text();
    const name = $("[id*=lblItemName]", $panel).text();
    const labelName = $("[id*=txtLabelName]", $panel).val();
    const size = $("[id*=txtSize]", $panel).val();
    const color = $("[id*=txtColor]", $panel).val();
    const txtRem1 = $("[id*=txtRem1]", $panel).val();
    const txtRem2 = $("[id*=txtRem2]", $panel).val();

    const data = {
        form: {
            code: code,
            name: name,
            labelName: labelName,
            size: size,
            color: color,
            txtRem1: txtRem1,
            txtRem2: txtRem2
        }
    };

    //$.post("/XconService.asmx/UpdateItem", data)
    //.done(() => {
    //    console.log('return from doiiiii');
    //});

    WebForm_DoCallback("__page", "UPDATE_ITEM", () => {
        console.log('return from callback');
    }, this, null, true);

};

$(() => {
    $(document)
        .on("click", "[id=lnkSelect]", this.lnkSelectClick)
        .on("change", "[id=txtQty]", this.txtQtyChanged)
        .on("click", "[id=btnCancel]", this.btnCancelClick)
        .on("click", "[id=btnUpdate]", this.btnUpdateClick)
    ;
});

function uploadComplete(e, a) {
    console.log(e, a);
}

function uploadError(e, a) {
    console.log(e, a);
}
