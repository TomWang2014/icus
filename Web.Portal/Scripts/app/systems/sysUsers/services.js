var service = service || {};

(function () {
    var adminService = function (serviceName) {
        this.name = serviceName || "UnnamedService";
        this.pageSzie = 10;
    };

    adminService.prototype.dto = {
        // 新增或者编辑模型
        sysUserDto: function () {
            this.Id = 0;
            this.City = '';
            this.IdentityCard = '';//身份证号
            this.Email = '';//邮箱
            this.Phone = '';//手机号
            this.RealName = '';//真实姓名
            this.CompanyName = '';//公司名称
            this.CompanyId = 0;//公司id
            this.License = '';//营业执照编号
            this.SellerID = '';//所属销售id
            this.BuyCount = '';//购买条数
            this.BuyPrice = '';//购买金额
            this.Percentage = '';//提成比例
            this.DealImg = '';//交易图片地址
            
        },
        sysSellerDto:function(){
            this.Id = 0;
            this.S_IdentityCard = '';//身份证号
            this.S_Email = '';//邮箱
            this.S_Phone = '';//手机号
            this.S_RealName = '';//真实姓名
            this.S_BankCard = '';//银行卡号
            this.S_BankName = '';//开户银行名称
        },
        //搜索条件
        sysSearchDto: function () {
            this.PageIndex = 1;
            this.PageSize = 10;
            this.Role = 0;
            this.City = '';
            this.RealName = '';
            this.Company = '';
            this.UserID = '';
            this.Phone = '';
            this.StartTime = '';
            this.EndTime = '';
            this.Orderby = "CreateTime";
            this.Desc = true;
        }
    };

    adminService.prototype.io = {

        // 修改用户信息
        modifyUserInfo: function (model) {
            return $.ajax({
                url: '/system/modifyUserInfo',
                data: { "model": model },
                type: "post",
                cache: false
            });
        },

        // 获得用户集合
        getList: function (model) {
            return $.ajax({
                url: '/System/GetALlUserList',
                data: { model: model },
                type: "get",
                cache: false
            });
        },

        // 删除用户
        deleteuser: function (id) {
            return $.ajax({
                url: '/System/DeleteUser',
                data: { "id": id },
                type: "post",
                cache: false
            });
        },

        // 删除用户
        deleteBatch: function (ids) {
            return $.ajax({
                url: '/System/DeleteBatch',
                data: { "ids": ids },
                type: "post",
                cache: false
            });
        },

    };

    service.sysUser = service.sysUser || new adminService("admin-service");
})();