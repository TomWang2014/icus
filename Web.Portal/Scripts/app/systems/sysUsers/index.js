var vm = new Vue({
    el: 'body',
    data: {
        // 编辑用户dto
        dto: new service.sysUser.dto.sysUserDto(),
        //搜索dto
        sysSearchDto: new service.sysUser.dto.sysSearchDto(),
        list: [],
        current: 0
    },
    methods: {

        // 编辑用户信息
        save: function () {
            var model = JSON.stringify(this.dto);
            // 表单是否通过前端验证
            if (ICusCRM.Validator.isValid("#signupForm")) {
                service.sysUser.io.modifyUserInfo(model).then(function() {
                    $("#myModal").modal("hide");
                    ICusCRM.Msg.showSuccess("编辑用户成功");
                    vm.getList();
                });
            }
        },

        // 编辑用户
        edituser: function (el) {
            console.log(this.dto);
            this.dto = JSON.parse(JSON.stringify(el));
            $("#myModal").modal("show");
        },

        // 获得用户列表
        getList: function () {

            var modelstr = JSON.stringify(this.sysSearchDto);
            service.sysUser.io.getList(modelstr).then(function (data) {

                vm.list = [];
                $(data.List).each(function (index, item) {
                    item.seleced = false;
                    vm.list.push(item);
                });
                $("#pager").pager(data.PageIndex, data.PageCount, data.PageSize, data.RecordCount);
            });
        }
    },
    ready: function () {

        this.getList();
        modelHidden();

        $("#pager").pager().bind("changed.pager", function (e, newPage) {
            vm.sysSearchDto.PageIndex = newPage;
            vm.getList();
        });
    }
});

function modelHidden() {

    // 绑定窗口关闭事件
    $('#myModal').on('hide.bs.modal', function () {
        // 清空vm
        vm.dto = new service.sysUser.dto.sysUserDto();
        $("#signupForm").data('bootstrapValidator').resetForm();
    });
}