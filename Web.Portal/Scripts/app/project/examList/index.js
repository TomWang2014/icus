﻿
var vm = new Vue({
    el: 'body',
    data: {
        SearchDto: new service.project.dto.seachDto(),
        list: [],//学员
        listSelected: []
    },
    methods: {
        //根据搜索条件获取学员
        getList: function () {
            service.project.io.getList(JSON.stringify(this.SearchDto)).then(function (data) {
                vm.list = [];
                vm.listSelected = [];
                $(data.List).each(function (index, item) {
                    item.seleced = false;
                    vm.list.push(item);
                });
                $("#pager").pager(data.PageIndex, data.PageCount, data.PageSize, data.RecordCount);
            });
        },
        //跳转到学员信息页面
        StudentInfo: function (id) {
            window.document.location.href = "/Student/StudentDetail?Id=" + id;
        },
        //审核通过/审核不通过
        setExamStatus: function (id, type) {
            service.project.io.setExamStatus(id, type).then(function (data) {
                ICusCRM.Msg.showSuccess();
                vm.getList();
            });
        },
        // checkbox复选
        change: function (that, id) {
            if (that) {
                this.listSelected.push(id);
            } else {
                this.listSelected.splice(this.listSelected.indexOf(id), 1);
            }
        },
        // 批量审核通过
        PassBatch: function () {
            var ids = JSON.stringify(this.listSelected);
            service.project.io.ExamPassBatch(ids).then(function () {
                ICusCRM.Msg.showSuccess();
                vm.getList();
            });
        },
        // 批量审核不通过
        UnPassBatch: function () {
            var ids = JSON.stringify(this.listSelected);
            service.project.io.ExamUnPassBatch(ids).then(function () {
                ICusCRM.Msg.showSuccess();
                vm.getList();
            });
        }

    },
    computed: {
        checkAllBool: {
            get: function () {
                return this.list.reduce(function (prev, curr) {
                    return prev && curr.seleced;
                }, true);
            },
            set: function (newValue) {
                $(this.list).each(function (index, item) {
                    if (newValue) {
                        if (vm.listSelected.indexOf(item.Id) < 0) {
                            vm.listSelected.push(item.Id);
                        }
                    } else {
                        vm.listSelected = [];
                    }
                    item.seleced = newValue;
                });
            }
        }
    },
    watch: {

    },
    ready: function () {
        this.getList();
        $("#pager").pager().bind("changed.pager", function (e, newPage) {
            vm.SearchDto.PageIndex = newPage;
            vm.getList();
        });

    }
});