
var vm = new Vue({
    el: 'body',
    data: {
        SearchDto: new service.student.dto.seachDto(),
        examlist: [],//考试记录
        paylist: [],//缴费记录
        learnlist: []//学习记录
    },
    methods: {
        //获取学员得考试记录
        getExamList: function () {
            service.student.io.getExamList().then(function (data) {
                vm.examlist = [];
                $(data.List).each(function (index, item) {
                    vm.examlist.push(item);
                });
            });
        },
        //获取学员得缴费记录
        getPayList: function () {
            service.student.io.getPayList().then(function (data) {
                vm.paylist = [];
                $(data.List).each(function (index, item) {
                    vm.paylist.push(item);
                });
            });
        },
        getLearnList: function () {
            service.student.io.getLearnList(JSON.stringify(this.SearchDto)).then(function (data) {
                vm.learnlist = [];
                $(data.List).each(function (index, item) {
                    vm.learnlist.push(item);
                });
                $("#pager").pager(data.PageIndex, data.PageCount, data.PageSize, data.RecordCount);
            });
        },
        // 排序
        sort: function (filds) {
            this.SearchDto.Orderby = filds;
            this.SearchDto.Desc = !this.SearchDto.Desc;
            this.getList();
        }
    },
    watch: {

    },
    ready: function () {
        this.getExamList();
        this.getPayList();
        this.getLearnList();
        $("#pager").pager().bind("changed.pager", function (e, newPage) {
            vm.SearchDto.PageIndex = newPage;
            vm.getLearnList();
        });

    }
});