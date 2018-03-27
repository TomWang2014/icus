/*
 *------------------------------------------------------------------
 模块描述说明：
 新闻管理功能模块对应的js文件
 作者：zw
 时间：2016-11-11 09:42:53
 ----------------------------------------------------------------- */
var vm = new Vue({
    el: 'body',
    data: {
        // 编辑用户dto
        dto: new service.newsInfo.dto.newsInfoDto(),
        SearchDto: new service.newsInfo.dto.seachDto(),
        typeSearchDto: new service.newsType.dto.seachDto(),
        current: 0,
        list: [],
        newsTypeList: [],
        listSelected: []
    },
    methods: {
        getList: function () {
            service.newsInfo.io.getList(JSON.stringify(this.SearchDto)).then(function (data) {
                vm.list = [];
                vm.listSelected = [];
                $(data.List).each(function (index, item) {
                    item.seleced = false;
                    vm.list.push(item);
                });
                $("#pager").pager(data.PageIndex, data.PageCount, data.PageSize, data.RecordCount);
            });
        },
        addnewsInfo: function () {
            window.document.location.href = "/News/AddNewsInfo?Id=0";
        },
        editnewsinfo: function (id) {
            window.document.location.href = "/News/AddNewsInfo?Id=" + id;
        },
        //发布新闻
        pubnewsinfo: function (id) {
            service.newsInfo.io.pubNewsInfo(id).then(function () {
                college.Msg.showSuccess();
                vm.getList();
            });
        },
        //取消发布
        unpubnewsinfo:function (id) {
            service.newsInfo.io.unpubNewsInfo(id).then(function () {
                college.Msg.showSuccess();
                vm.getList();
            });
        },
        //获取新闻栏目列表
        getNewsTypeList: function () {
            service.newsType.io.getList(JSON.stringify(this.typeSearchDto)).then(function (data) {
                vm.newsTypeList = data.List;
            });
        },
        // 删除新闻
        deleteNewsInfo: function (id) {
            service.newsInfo.io.deleteNewsInfo(id).then(function () {
                college.Msg.showSuccess();
                vm.getList();
            });
        },
        // 批量删除
        deleteBatch: function () {
            var ids = JSON.stringify(this.listSelected);
            service.newsInfo.io.deleteBatch(ids).then(function () {
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

        var pageKey = window.location.pathname;
        this.getNewsTypeList();

        var pageValue = $.cookie(pageKey);
        if (pageValue) {
            this.SearchDto.PageIndex = pageValue;
        }

        this.getList();
        modelHidden();
        $("#pager").pager().bind("changed.pager", function (e, newPage) {
            vm.SearchDto.PageIndex = newPage;
            $.cookie(pageKey, newPage);
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
