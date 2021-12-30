/**
 * Utility for conditionally joining classNames together.
 * Alternative to the `classnames` and `clsx` modules.
 *
 * Example:
 * classNames(['icon', {'icon--filled': true, 'icon--empty': false}]); // => "icon icon--filled"
 *
 * @param {string[]|Object[]} input
 * @returns {string}
 */
function classNames(input) {
    const classes = [];

    input.forEach(value => {
        if (typeof value === 'string') classes.push(value);
        if (typeof value === 'object') {
            const keys = Object.keys(value);
            keys.forEach(function(key) {
                if (value[key]) classes.push(key);
            });
        }
    });

    return classes.join(' ');
}

export { classNames };
