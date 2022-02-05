//使用者相關
//角色
const LoadUserRoles = () => {
    return req.get(`Admin/LoadUserRoles`);
};
//新增編輯角色
const AddOrEditUserRole = (data) => {
    return req.post(`Admin/AddOrEditUserRole`, data);
};

//使用者
//載入使用者
const LoadUsers = () => {
    return req.get(`Admin/LoadUsers`);
};
//載入使用者
const LoadUserByLoginUser = () => {
    return req.get(`Admin/LoadUserByLoginUser`);
};
//新增編輯使用者
const AddOrEditUser = (data) => {
    return req.post(`Admin/AddOrEditUser`, data);
};

//留言板
//發送訊息
const CreatMessage = (data) => {
    return req.post(`MessageBoard/CreatMessage`, data);
};
//訊息提醒
const MessageStatus = () => {
    return req.get(`MessageBoard/MessageStatus`);
};