if (typeof String.prototype.format != 'function') {
    String.prototype.format = function () {
        var args = arguments;
        if (typeof args[0] == "object")
            args = args[0];

        return this.replace(/{([\d\w\_\-]+)}/g, function (match, number) {
            return typeof args[number] != 'undefined'
				? args[number]
				: match
            ;
        });
    };
}
