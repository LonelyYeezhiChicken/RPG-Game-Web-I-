
window.root = window.root ? window.root : '/';

const apiURL = window.root;

const instance = axios.create({
    baseURL: apiURL + 'api/',
    headers: getAuthorizationHeader(),
    timeout: 40000
});

function getAuthorizationHeader() {
    const GMTString = new Date().toGMTString();

    return {
        // 'Accept-Encoding': 'gzip',
        'Content-Type': 'application/json',
        'X-Date': GMTString
    };
}

instance.interceptors.request.use(
    function (config) {
        // Do something before request is sent
        // e.g. show loading
        return config;
    },
    function (error) {
        // Do something with request error
        return Promise.reject(error);
    }
);

instance.interceptors.response.use(
    function (response) {
        return response;
    },
    function (error) {
        if (error.response) {
            switch (error.response.status) {
                case 400:
                    console.log(error.message);
                    return error.message;
                case 404:
                    console.log('你要找的頁面不存在');
                    break;
                case 500:
                    console.log('程式發生問題');
                    break;
                default:
                    console.log(error.message);
            }
        }
        if (!window.navigator.onLine) {
            alert('網路出了點問題，請重新連線後重整網頁');
            return;
        }
        return Promise.reject(error);
    }
);

function get(url, data = null, config) {
    return instance.get(url, { params: { ...data }, ...config });
}

function post(url, data = null, config) {
    return instance.post(url, data, config);
}

function deleteRequest(url, data = null, config) {
    return instance.delete(url, { params: data, ...config });
}

function put(url, data = null, config) {
    return instance.put(url, data, config);
}

function patch(url, data = null, config) {
    return instance.patch(url, data, config);
}

const req = {
    get,
    post,
    delete: deleteRequest,
    put,
    patch
};


