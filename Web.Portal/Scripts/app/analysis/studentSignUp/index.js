
var vm = new Vue({
    el: 'body',
    data: {
        SearchDto: new service.analysis.dto.seachDto(),
        list: [],//学员
        tenantList: [],//合作机构
        projectList: [],//项目  
        ids:[]
    },
    methods: {
        //根据搜索条件获取学员
        getList: function () {
            service.analysis.io.getList(JSON.stringify(this.SearchDto)).then(function (data) {
                vm.list = [];
                if (data.PageIndex == 1) {
                    vm.ids = [];
                }
                $(data.List).each(function (index, item) {
                    if (vm.ids.indexOf(item.ProjectId) <= -1) {
                        vm.list.push(item);
                        vm.ids.push(item.ProjectId);
                    }
                });
                $("#pager").pager(data.PageIndex, data.PageCount, data.PageSize, data.RecordCount);
            });
        },
        //获取合作机构
        getTenantList: function () {
            service.analysis.io.getTenantList().then(function (data) {
                vm.tenantList = data;
            });
        },
        //获取项目
        getProjectList: function () {
            service.analysis.io.getProjectList(JSON.stringify(this.SearchDto)).then(function (data) {
                vm.projectList = data;
            });
        },
        //跳转到学员信息页面
        StudentsInfo: function (id, tenantName, projectName) {
            window.document.location.href = "/Project/StudentList?projectId=" + id + "&tenantName=" + tenantName + "&projectName=" + projectName;
        },
        outRepeat: function (arr) {

        }


    },
    watch: {

    },
    ready: function () {
        this.getTenantList();
        this.getProjectList();
        this.getList();
        $("#pager").pager().bind("changed.pager", function (e, newPage) {
            vm.SearchDto.PageIndex = newPage;
            vm.getList();
        });

    }
});