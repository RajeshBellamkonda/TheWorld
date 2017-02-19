//simpleControls.js
(function () {
    "use strict";

    angular.module("simpleControls", [])
    .directive("waitCursor", waitCursor);

    function waitCursor() {
        return {
            scope:{
                show:"=displayWhen"
            },
            // restricts the only to element style ie. <wait-cursor></wait-cursor>
            // not attribure style <div wait-cursor></div>
            resstict: "E",             
            templateUrl: "/views/waitCursor.html"
        };
    }

})();