/*
 *------------------------------------------------------------------
 模块描述说明：
 新闻栏目管理功能模块对应的js文件
 作者：zw
 时间：2016-11-11 09:42:53
 ----------------------------------------------------------------- */
var vm = new Vue({
    el: 'body',
    data: {
        // 编辑用户dto
        dto: new service.newsType.dto.newsTypeDto(),
        SearchDto: new service.newsType.dto.seachDto(),
        current: 0,
        list: [],
        listSelected: []
    },
    methods: {
        getList: function () {
            service.newsType.io.getList(JSON.stringify(this.SearchDto)).then(function (data) {
                vm.list = [];
                vm.listSelected = [];
                $(data.List).each(function (index, item) {
                    item.seleced = false;
                    vm.list.push(item);
                });
                $("#pager").pager(data.PageIndex, data.PageCount, data.PageSize, data.RecordCount);
            });
        },
        editNewsType: function (el) {
            this.dto = JSON.parse(JSON.stringify(el));
            $("#myModal").modal("show");
        },
        save: function () {
            if (college.Validator.isValid("#signupForm")) {
                service.newsType.io.modifyNewsType(JSON.stringify(this.dto)).then(function () {
                    $("#myModal").modal("hide");
                    college.Msg.showSuccess("编辑新闻栏目成功");
                    vm.getList();
                });
            }
        },

        // 删除栏目
        deleteNewsType: function (id) {
            service.newsType.io.deleteNewsType(id).then(function () {
                college.Msg.showSuccess();
                vm.getList();
            });
        },

        // 批量删除
        deleteBatch: function () {
            var ids = JSON.stringify(this.listSelected);
            service.newsType.io.deleteBatch(ids).then(function () {
                college.Msg.showSuccess();
                vm.getList();
            });
        },

        // 排序
        sort: function (filds) {
            this.SearchDto.Orderby = filds;
            this.SearchDto.Desc = !this.SearchDto.Desc;
            this.getList();
        },

        // checkbox复选
        change: function (that, id) {
            if (that) {
                this.listSelected.push(id);
            } else {
                this.listSelected.splice(this.listSelected.indexOf(id), 1);
            }
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
    ready: function () {
        this.getList();
        modelHidden();

        $("#pager").pager().bind("changed.pager", function (e, newPage) {
            vm.SearchDto.PageIndex = newPage;
            vm.getList();
        });
    }
});

function modelHidden() {

    // 绑定窗口关闭事件
    $('#myModal').on('hidden.bs.modal', function () {
        // 清空vm
        vm.dto = new service.sysUser.dto.sysUserDto();
    });
}