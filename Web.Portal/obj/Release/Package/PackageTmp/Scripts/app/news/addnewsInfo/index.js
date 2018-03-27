/*
 *------------------------------------------------------------------
 模块描述说明：
 新增新闻功能模块对应的js文件
 作者：zw
 时间：2016-11-11 09:42:53
 ----------------------------------------------------------------- */
var vm = new Vue({
    el: 'body',
    data: {
        // 编辑用户dto
        dto: new service.addnewsInfo.dto.addnewsInfoDto(),
        typeSearchDto: new service.newsType.dto.seachDto(),
        logoMsg: "新闻缩略图是新闻列表显示的小图片，请选择图片后点击编辑并上传即可。",
        newsTypeList: []
    },
    methods: {
        //获取新闻栏目列表
        getNewsTypeList: function () {
            service.newsType.io.getList(JSON.stringify(this.typeSearchDto)).then(function (data) {
                vm.newsTypeList = data.List;
                Vue.nextTick(function () {
                    SetIschecks($(".i-checks"));
                });
            });
        },
        setNewsInfo: function (id) {
            service.addnewsInfo.io.GetNewsInfoDetail(id).then(function (data) {
                vm.dto = data;
                $("input[name='IsTop'][value='" + vm.dto.IsTop + "']").iCheck('check');
                if (vm.dto.NewsTypeIds != null) {
                    $.each(vm.dto.NewsTypeIds, function (i, n) {
                        $("input[name='newsType'][value='" + n + "']").iCheck('check');
                    });
                }
                editor.$txt.html(vm.dto.Contect);
            });
        },
        delnewsinfoimg: function () {
            vm.dto.Thumbnail = "";
        },
        save: function () {
            if (college.Validator.isValid("#signupForm")) {
                vm.dto.IsTop = $("input[name='IsTop']:checked").val();
                vm.dto.NewsTypeIds = [];
                $("input[name='newsType']:checked").each(function () {
                    vm.dto.NewsTypeIds.push($(this).val());
                });
                if ($("input[name='IsAbstract']:checked").val() == "1") {
                    var sbstr = editor.$txt.formatText();
                    vm.dto.Abstract = sbstr.length <= 200 ? sbstr : sbstr.substring(0, 200);
                }
                service.addnewsInfo.io.modifyNewsInfo(JSON.stringify(this.dto)).then(function () {
                    if (vm.dto.Id > 0) {
                        college.Msg.showSuccess("编辑新闻成功");
                        setTimeout(function () {
                            window.document.location.href = "/News/NewsInfoMgtResult";
                        }, 1000);
                    }
                    else {
                        college.Msg.showSuccess("新增新闻成功");
                        setTimeout(function () {
                            if (window.document.location.href.indexOf("/News/AddNewsInfo?Id") > -1) {
                                window.document.location.href = "/News/NewsInfoMgtResult";
                            }
                            else {
                                vm.cancel();
                            }
                        }, 1000);
                    }
                });
            }
        },
        cancel: function () {
            if (window.document.location.href.indexOf("/News/AddNewsInfo?Id") > -1) {
                window.document.location.href = "/News/NewsInfoMgtResult";
            }
            else {
                window.parent.ToPage(true, "/News/NewsInfoMgtResult");
            }
        }
    },
    ready: function () {
        this.getNewsTypeList();
        if (objId != "0") {
            this.setNewsInfo(objId);
        }
        // 创建编辑器
        editor = new wangEditor('editor-trigger');
        editor.config.mapAk = 'TVhjYjq1ICT2qqL5LdS8mwas';
        wangEditor.config.printLog = false; // 阻止输出log
        //editor.config.pasteText  = true;//只粘贴纯文本
        editor.config.uploadImgUrl = '/News/upload';
        editor.config.uploadHeaders = {
            'Accept': 'text/x-json'
        };//设置 headers
        editor.config.uploadParams = {
            path: "/NewsImg/"
        };
        editor.onchange = function () {
            vm.dto.Contect = editor.$txt.html();
        };
        editor.create();

        $('#avatarForm').ajaxForm({
            beforeSubmit: function () {
                if ($("#avatarInput").val().length < 1)
                    return false;
            },
            success: function (data) {
                if (data.state == "200") {
                    college.Msg.showSuccess("上传新闻缩略图成功");
                    vm.dto.Thumbnail = data.imgurl;
                    $("#myModal").modal("hide");
                }
                else {
                    college.Msg.showError("上传新闻缩略图失败");
                }
            }
        });

    }
});

var editor;//编辑器

//绑定单选框 复选框样式
function SetIschecks(obj) {
    obj.iCheck({
        checkboxClass: 'icheckbox_square-green',
        radioClass: 'iradio_square-green',
    });
    obj.find("input[name='IsAbstract'][value='1']").on("ifChecked", function () {
        $("#Abstract").val('');
        $('#Abstract').attr("disabled", "disabled");
    }).on("ifUnchecked", function () {
        $('#Abstract').removeAttr("disabled");
    });
}

$("#btnSelectFile").click(function () {
    $("#avatarInput").click();
});

