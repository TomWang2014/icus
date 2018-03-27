/*
 *------------------------------------------------------------------
 模块描述说明：
 用户管理功能模块对应的js文件
 作者：李天赐
 时间：2016-11-11 09:42:53
 ----------------------------------------------------------------- */
var vm = new Vue({
    el: 'body',
    data: {
        sysSearchDto: new service.project.dto.sysSearchDto(),
        content: '',
        types: [],
        list: []
    },
    methods: {
        // 获得用户列表
        getList: function () {
            var modelstr = JSON.stringify(this.sysSearchDto);
            service.project.io.getList(modelstr).then(function (data) {
                vm.list = data.List;
                $("#pager").pager(data.PageIndex, data.PageCount, data.PageSize, data.RecordCount);
            });
        },

        submitAudit: function (id) {
            service.project.io.submitAudit(id).then(function (data) {
                ICusCRM.Msg.showSuccess();
                vm.getList();
            });
        },
        withdrawalAudit: function (id) {
            service.project.io.withdrawalAudit(id).then(function (data) {
                ICusCRM.Msg.showSuccess();
                vm.getList();
            });
        },
        // 删除
        deleteItem: function (id) {
            service.project.io.deleteItem(id).then(function () {
                ICusCRM.Msg.showSuccess();
                vm.getList();
            });
        },

        // 获得角色列表
        getType: function () {
            service.project.io.getType().then(function (data) {
                vm.types = data;
            });
        }
    },
    ready: function () {

        var pageKey = window.location.pathname;
        var pageValue = $.cookie(pageKey);
        if (pageValue) {
            this.sysSearchDto.PageIndex = pageValue;
        }

        this.getList();
        this.getType();

        // 分页
        $("#pager").pager().bind("changed.pager", function (e, newPage) {
            $.cookie(pageKey, newPage);
            vm.sysSearchDto.PageIndex = newPage;
            vm.getList();
        });
    }
});





