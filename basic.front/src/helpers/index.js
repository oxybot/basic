// Based on: https://stackoverflow.com/a/34890276/17681099
export function groupBy(xs, key) {
    if (typeof key === 'function') {
        return xs.reduce(function (rv, x) {
            (rv[key(x)] = rv[key(x)] || []).push(x);
            return rv;
        }, {});
    } else {
        return xs.reduce(function (rv, x) {
            (rv[x[key]] = rv[x[key]] || []).push(x);
            return rv;
        }, {});
    }
}

// Based on: https://stackoverflow.com/a/14810722/17681099
export function objectMap(obj, fn) {
    return Object.entries(obj).map(
        ([k, v], i) => fn(v, k, i)
    )
}
