/*
cms.sort.js
七种排序算法:quickSort,heapSort,mergeSort,shellSort,binaryInsertSort,insertSort,bubbleSort,selectSort
基于数字比较大小，若为字符型数字需转换为数字
一般情况下 选择快速排序（quickSort），当数组长度小于10时，可选择插入排序（insertSort)
*/
var cms = cms || {};
cms.sort = {};

cms.sort.checkKey = function (key) {
    return typeof key == 'string' || (typeof key == 'number' && key >= 0);
};

/*快速排序(递归方式)*/
cms.sort.quickSort = function (arr, key) {
    if (0 == arr.length) {
        return arr;
    }
    return cms.sort.checkKey(key) ? cms.sort._quickSortKey(arr, key) : cms.sort._quickSort(arr);
};

cms.sort._quickSort = function (arr) {
    if (0 == arr.length) {
        return [];
    }
    var left = [], right = [], pivot = arr[0], c = arr.length;
    for (var i = 1; i < c; i++) {
        arr[i] < pivot ? left.push(arr[i]) : right.push(arr[i]);
    }
    return cms.sort._quickSort(left).concat(pivot, cms.sort._quickSort(right));
};

cms.sort._quickSortKey = function (arr, key) {
    if (0 == arr.length) {
        return [];
    }
    var left = [], right = [], pivot = arr[0], c = arr.length;
    for (var i = 1; i < c; i++) {
        arr[i][key] < pivot[key] ? left.push(arr[i]) : right.push(arr[i]);
    }
    return cms.sort._quickSortKey(left, key).concat(pivot, cms.sort._quickSortKey(right, key));
};

/*堆排序*/
cms.sort.heapSort = function (arr, key) {
    var c = arr.length, hasKey = cms.sort.checkKey(key);
    for (var i = Math.floor(c / 2); i >= 0; i--) {
        cms.sort.heapAdjust(arr, i, c - 1, hasKey, key);
    }
    for (var i = c - 1; i >= 0; i--) {
        cms.sort.swap(arr, 0, i);
        cms.sort.heapAdjust(arr, 0, i - 1, hasKey, key);
    }
    return arr;
};

cms.sort.heapAdjust = function (arr, s, m, hasKey, key) {
    var tmp = arr[s];
    if (hasKey) {
        for (var j = 2 * s; j <= m; j *= 2) {
            j += j < m && arr[j][key] < arr[j + 1][key] ? 1 : 0;
            if (tmp[key] >= arr[j][key]) {
                break;
            }
            arr[s] = arr[j], s = j;
        }
    } else {
        for (var j = 2 * s; j <= m; j *= 2) {
            j += j < m && arr[j] < arr[j + 1] ? 1 : 0;
            if (tmp >= arr[j]) {
                break;
            }
            arr[s] = arr[j], s = j;
        }
    }
    arr[s] = tmp;
};

/*归并排序(递归方式)*/
cms.sort.mergeSort = function (arr, key) {
    return cms.sort._mergeSort(arr, arr, 0, arr.length - 1, cms.sort.checkKey(key), key);
};

cms.sort._mergeSort = function (arrSource, arrTarget, s, t, hasKey, key) {
    var m = 0, arrTarget2 = [];
    if (s == t) {
        arrTarget[s] = arrSource[s];
    } else {
        m = Math.floor((s + t) / 2);

        cms.sort._mergeSort(arrSource, arrTarget2, s, m, hasKey, key);
        cms.sort._mergeSort(arrSource, arrTarget2, m + 1, t, hasKey, key);
        cms.sort._merge(arrTarget2, arrTarget, s, m, t, hasKey, key);
    }
    return arrTarget;
};

cms.sort._merge = function (arrSource, arrTarget, s, m, n, hasKey, key) {
    var i = 0, k = 0;
    if (hasKey) {
        for (i = m + 1, k = s; s <= m && i <= n; k++) {
            arrTarget[k] = arrSource[arrSource[s][key] < arrSource[i][key] ? s++ : i++];
        }
    } else {
        for (i = m + 1, k = s; s <= m && i <= n; k++) {
            arrTarget[k] = arrSource[arrSource[s] < arrSource[i] ? s++ : i++];
        }
    }
    if (s <= m) {
        for (var j = 0; j <= m - s; j++) {
            arrTarget[k + j] = arrSource[s + j];
        }
    }
    if (i <= n) {
        for (var j = 0; j <= n - i; j++) {
            arrTarget[k + j] = arrSource[i + j];
        }
    }
};

/*归并排序(非递归方式)*/
cms.sort.mergeSort2 = function (arr, key) {
    var c = arr.length, k = 1, arrTarget = [];
    while (k < c) {
        cms.sort._mergeSort2(arr, arrTarget, k, c - 1, cms.sort.checkKey(key), key);
        k = 2 * k;
        cms.sort._mergeSort2(arrTarget, arr, k, c - 1, cms.sort.checkKey(key), key);
        k = 2 * k;
    }
    return arr;
};

cms.sort._mergeSort2 = function (arrSource, arrTarget, s, t, hasKey, key) {
    var i = 0, j = 0;
    while (i <= t - 2 * s + 1) {
        cms.sort._merge(arrSource, arrTarget, i, i + s - 1, i + 2 * s - 1, hasKey, key);
        i += 2 * s;
    }
    if (i < t - s + 1) {
        cms.sort._merge(arrSource, arrTarget, i, i + s - 1, t, hasKey, key);
    } else {
        for (j = i; j <= t; j++) {
            arrTarget[j] = arrSource[j];
        }
    }
};

/*希尔排序*/
cms.sort.shellSort = function (arr, key) {
    if (!cms.sort.checkKey(key)) {
        var tmp = 0, c = arr.length, increment = c;
        do {
            increment = Math.floor(increment / 3) + 1;
            for (var i = increment; i < c; i++) {
                if (arr[i] < arr[i - increment]) {
                    tmp = arr[i];
                    for (var j = i - increment; j >= 0 && arr[j] > tmp; j -= increment) {
                        arr[j + increment] = arr[j];
                    }
                    arr[j + increment] = tmp;
                }
            }
        } while (increment > 1);
        return arr;
    } else {
        return cms.sort.shellSortKey(arr, key);
    }
};

cms.sort.shellSortKey = function (arr, key) {
    var tmp = 0, c = arr.length, increment = c;
    do {
        increment = Math.floor(increment / 3) + 1;
        for (var i = increment; i < c; i++) {
            if (arr[i][key] < arr[i - increment][key]) {
                tmp = arr[i];
                for (var j = i - increment; j >= 0 && arr[j][key] > tmp[key]; j -= increment) {
                    arr[j + increment] = arr[j];
                }
                arr[j + increment] = tmp;
            }
        }
    } while (increment > 1);
    return arr;
};

/*插入排序*/
cms.sort.insertSort = function (arr, key) {
    var tmp = 0, c = arr.length, hasKey = cms.sort.checkKey(key);
    if (hasKey) {
        for (var i = 1; i < c; i++) {
            tmp = arr[i];
            for (var j = i; j > 0 && arr[j - 1][key] > tmp[key]; j--) {
                arr[j] = arr[j - 1];
            }
            arr[j] = tmp;
        }
    } else {
        for (var i = 1; i < c; i++) {
            tmp = arr[i];
            for (var j = i; j > 0 && arr[j - 1] > tmp; j--) {
                arr[j] = arr[j - 1];
            }
            arr[j] = tmp;
        }
    }
    return arr;
};

/*折半插入排序*/
cms.sort.binaryInsertSort = function (arr, key) {
    var c = arr.length, low = 0, high = 0, mid = 0, tmp = 0, hasKey = cms.sort.checkKey(key);
    for (var i = 1; i < c; i++) {
        tmp = arr[i];
        low = 0;
        high = i - 1;
        if (hasKey) {
            while (low <= high) {
                mid = Math.floor((low + high) / 2);
                tmp[key] > arr[mid][key] ? low = mid + 1 : high = mid - 1;
            } 
        } else {
            while (low <= high) {
                mid = Math.floor((low + high) / 2);
                tmp > arr[mid] ? low = mid + 1 : high = mid - 1;
            } 
        }
        for (var j = i - 1; j > high; j--) {
            arr[j + 1] = arr[j];
        }
        arr[high + 1] = tmp;
    }
    return arr;
};

cms.sort.swap = function (arr, i, j) {
    var tmp = arr[i];
    arr[i] = arr[j];
    arr[j] = tmp;
};

/*选择排序*/
cms.sort.selectSort = function (arr, key) {
    var c = arr.length, min = 0, hasKey = cms.sort.checkKey(key);
    if (hasKey) {
        for (var i = 0; i < c - 1; i++) {
            min = i;
            for (var j = i + 1; j < c; j++) {
                arr[min][key] > arr[j][key] ? min = j : 0;
            }
            i != min ? cms.sort.swap(arr, i, min) : 0;
        }
    } else {
        for (var i = 0; i < c - 1; i++) {
            min = i;
            for (var j = i + 1; j < c; j++) {
                arr[min] > arr[j] ? min = j : 0;
            }
            i != min ? cms.sort.swap(arr, i, min) : 0;
        }
    }
    return arr;
};

/*冒泡排序*/
cms.sort.bubbleSort = function (arr, key) {
    var c = arr.length, flag = true, hasKey = cms.sort.checkKey(key);
    if (hasKey) {
        for (var i = 0; i < c - 1 && flag; i++) {
            flag = false;
            for (var j = c - 1; j > i; j--) {
                arr[j][key] < arr[j - 1][key] ? flag = cms.sort.swap(arr, j, j - 1) || true : 0;
            }
        }
    } else {
        for (var i = 0; i < c - 1 && flag; i++) {
            flag = false;
            for (var j = c - 1; j > i; j--) {
                arr[j] < arr[j - 1] ? flag = cms.sort.swap(arr, j, j - 1) || true : 0;
            }
        }
    }
    return arr;
};