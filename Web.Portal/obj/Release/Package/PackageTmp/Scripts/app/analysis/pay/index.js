
var vm = new Vue({
    el: 'body',
    data: {
        SearchDto: new service.analysis.dto.seachDto(),
        list: [],//缴费
        tenantList: [],//合作机构
        projectList: [],//项目        
    },
    methods: {
        //根据搜索条件获取学习记录
        getList: function () {
            service.analysis.io.getList(JSON.stringify(this.SearchDto)).then(function (data) {
                vm.list = [];
                $(data.List).each(function (index, item) {
                    vm.list.push(item);
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