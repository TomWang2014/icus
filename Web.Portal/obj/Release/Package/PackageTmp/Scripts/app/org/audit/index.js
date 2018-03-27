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
        sysSearchDto: new service.audit.dto.sysSearchDto(),
        content: '',
        types: [],
        list: []
    },
    methods: {
        // 获得用户列表
        getList: function () {
            var modelstr = JSON.stringify(this.sysSearchDto);
            service.audit.io.getList(modelstr).then(function (data) {
                vm.list = data.List;
                $("#pager").pager(data.PageIndex, data.PageCount, data.PageSize, data.RecordCount);
            });
        },

        submitAudit: function (id) {
            service.audit.io.submitAudit(id).then(function (data) {
                college.Msg.showSuccess();
                vm.getList();
            });
        },
        adoptStatus: function (id) {
            service.audit.io.adoptStatus(id).then(function (data) {
                college.Msg.showSuccess();
                vm.getList();
            });
        },
        defeatedStatus: function (id) {
            service.audit.io.defeatedStatus(id).then(function (data) {
                college.Msg.showSuccess();
                vm.getList();
            });
        },
        // 获得角色列表
        getType: function () {
            service.audit.io.getType().then(function (data) {
                vm.types = data;
            });
        },
        // 设置冻结/解冻
        setFreezeStatus: function(id) {
            service.audit.io.setFreezeStatus(id).then(function (data) {
                college.Msg.showSuccess();
                vm.getList();
            });
        }
    },
    ready: function () {
        this.getList();
        this.getType();
        initEdit();
        fromSubmit();


        // 分页
        $("#pager").pager().bind("changed.pager", function (e, newPage) {
            vm.sysSearchDto.PageIndex = newPage;
            vm.getList();
        });
    }
});



var editor;//编辑器
function initEdit() {

    try {
        // 创建编辑器
        editor = new wangEditor('editor-trigger');
        editor.config.mapAk = 'TVhjYjq1ICT2qqL5LdS8mwas';
        wangEditor.config.printLog = false; // 阻止输出log
        editor.config.pasteText = true;//只粘贴纯文本
        editor.config.uploadImgUrl = '/News/upload';
        editor.config.uploadHeaders = {
            'Accept': 'text/x-json'
        };//设置 headers
        editor.onchange = function () {
            vm.content = editor.$txt.html();
        };
        editor.create();

    } catch (e) {

    }
}


function fromSubmit() {
    $("#signupForm").submit(function () {

        var model = {};
        $(this).ajaxSubmit({
            url: '/org/AddProject',
            dataType: 'text',
            beforeSubmit: function () {

            },
            success: function (path) {
                college.Msg.showSuccess("新增项目成功，即将跳转到列表页。");
                window.location.href = "/Org/ProjectMgtResul";
            }
        });
        return false;
    });
}