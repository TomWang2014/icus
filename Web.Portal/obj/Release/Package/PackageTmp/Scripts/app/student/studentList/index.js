/*
 *------------------------------------------------------------------
 模块描述说明：
 学员管理功能模块对应的js文件
 作者：wjh
 时间：2017-02-07
 ----------------------------------------------------------------- */
var vm = new Vue({
    el: 'body',
    data: {
        SearchDto: new service.student.dto.seachDto(),
        list: [],//学员
        tenantList: [],//合作机构
        projectList: [],//项目
        tenantName: ''
    },
    methods: {
        //根据搜索条件获取学员
        getList: function () {
            service.student.io.getList(JSON.stringify(this.SearchDto)).then(function (data) {
                vm.list = [];
                $(data.List).each(function (index, item) {
                    vm.list.push(item);
                });
                $("#pager").pager(data.PageIndex, data.PageCount, data.PageSize, data.RecordCount);
            });
        },
        //获取合作机构
        getTenantList: function () {
            service.student.io.getTenantList().then(function (data) {
                vm.tenantList = data;
            });
        },
        //获取项目
        getProjectList: function () {
            service.student.io.getProjectList(JSON.stringify(this.SearchDto)).then(function (data) {
                vm.projectList = data;
            });
        },
        // 排序
        sort: function (filds) {
            this.SearchDto.Orderby = filds;
            this.SearchDto.Desc = !this.SearchDto.Desc;
            this.getList();
        },
        //跳转到学员信息页面
        StudentInfo: function (id) {
            window.document.location.href = "/Student/StudentDetail?Id=" + id;
        },
        //跳转到学员信息编辑页面
        EditStudentInfo: function (id) {
            window.document.location.href = '/Student/EditStudentInfo?Id=' + id;
        },
        StudentHistory: function (studentNo) {
            window.document.location.href = '/Student/StudentHistory?StudentNo=' + studentNo;
        }


    },
    watch: {

    },
    ready: function () {
        this.getTenantList();
        this.getProjectList();

        var pageKey = window.location.pathname;
        var pageValue = $.cookie(pageKey);
        if (pageValue) {
            this.SearchDto.PageIndex = pageValue;
        }

        this.getList();
        $("#pager").pager().bind("changed.pager", function (e, newPage) {
            $.cookie(pageKey, newPage);
            vm.SearchDto.PageIndex = newPage;
            vm.getList();
        });

    }
});