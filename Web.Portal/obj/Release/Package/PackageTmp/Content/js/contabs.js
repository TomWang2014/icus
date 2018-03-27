
$(function () {
    //计算元素集合的总宽度
    function calSumWidth(elements) {
        var width = 0;
        $(elements).each(function () {
            //返回外边距
            width += $(this).outerWidth(true);
        });
        return width;
    }
    //滚动到指定选项卡
    function scrollToTab(element) {
        // 取得每个元素的前面的同胞元素，每个元素的所有跟随元素
        var marginLeftVal = calSumWidth($(element).prevAll());
        var marginRightVal = calSumWidth($(element).nextAll());
        // 可视区域非tab宽度，移除class为J_menuTabs的对象
        var tabOuterWidth = calSumWidth($(".content-tabs").children().not(".J_menuTabs"));
        //可视区域tab宽度
        var visibleWidth = $(".content-tabs").outerWidth(true) - tabOuterWidth;
        //实际滚动宽度
        var scrollVal = 0;
        if ($(".page-tabs-content").outerWidth() < visibleWidth) {
            scrollVal = 0;
        } else if (marginRightVal <= (visibleWidth - $(element).outerWidth(true) - $(element).next().outerWidth(true))) {
            if ((visibleWidth - $(element).next().outerWidth(true)) > marginRightVal) {
                scrollVal = marginLeftVal;
                var tabElement = element;
                while ((scrollVal - $(tabElement).outerWidth()) > ($(".page-tabs-content").outerWidth() - visibleWidth)) {
                    scrollVal -= $(tabElement).prev().outerWidth();
                    tabElement = $(tabElement).prev();
                }
            }
        } else if (marginLeftVal > (visibleWidth - $(element).outerWidth(true) - $(element).prev().outerWidth(true))) {
            scrollVal = marginLeftVal - $(element).prev().outerWidth(true);
        }
        $('.page-tabs-content').animate({
            marginLeft: 0 - scrollVal + 'px'
        }, "fast");
    }
    //查看左侧隐藏的选项卡
    function scrollTabLeft() {
        // 取绝对值，标签栏区左margin的距离
        var marginLeftVal = Math.abs(parseInt($('.page-tabs-content').css('margin-left')));
        // 可视区域非tab宽度
        var tabOuterWidth = calSumWidth($(".content-tabs").children().not(".J_menuTabs"));
        //可视区域tab宽度
        var visibleWidth = $(".content-tabs").outerWidth(true) - tabOuterWidth;
        //实际滚动宽度
        var scrollVal = 0;
        //假设当前tab集合的宽度小于他的最大宽度，则不作任何处理
        if ($(".page-tabs-content").width() < visibleWidth) {
            return false;
        } else {
            //获取tab集合中第一个tab元素
            var tabElement = $(".J_menuTab:first");
            var offsetVal = 0;
            //找到那个前方所有元素宽度总和大于当前显示宽度的元素
            while ((offsetVal + $(tabElement).outerWidth(true)) <= marginLeftVal) {//找到离当前tab最近的元素
                offsetVal += $(tabElement).outerWidth(true);
                tabElement = $(tabElement).next();
            }
            offsetVal = 0;
            if (calSumWidth($(tabElement).prevAll()) > visibleWidth) {
                while ((offsetVal + $(tabElement).outerWidth(true)) < (visibleWidth) && tabElement.length > 0) {
                    offsetVal += $(tabElement).outerWidth(true);
                    tabElement = $(tabElement).prev();
                }
                scrollVal = calSumWidth($(tabElement).prevAll());
            }
        }
        $('.page-tabs-content').animate({
            marginLeft: 0 - scrollVal + 'px'
        }, "fast");
    }
    //查看右侧隐藏的选项卡
    function scrollTabRight() {
        // 取绝对值，标签栏区左margin的距离
        var marginLeftVal = Math.abs(parseInt($('.page-tabs-content').css('margin-left')));
        // 可视区域非tab宽度
        var tabOuterWidth = calSumWidth($(".content-tabs").children().not(".J_menuTabs"));
        //可视区域tab宽度
        var visibleWidth = $(".content-tabs").outerWidth(true) - tabOuterWidth;
        //实际滚动宽度
        var scrollVal = 0;
        if ($(".page-tabs-content").width() < visibleWidth) {
            return false;
        } else {
            var tabElement = $(".J_menuTab:first");
            var offsetVal = 0;
            while ((offsetVal + $(tabElement).outerWidth(true)) <= marginLeftVal) {//找到离当前tab最近的元素
                offsetVal += $(tabElement).outerWidth(true);
                tabElement = $(tabElement).next();
            }
            offsetVal = 0;
            while ((offsetVal + $(tabElement).outerWidth(true)) < (visibleWidth) && tabElement.length > 0) {
                offsetVal += $(tabElement).outerWidth(true);
                tabElement = $(tabElement).next();
            }
            scrollVal = calSumWidth($(tabElement).prevAll());
            if (scrollVal > 0) {
                $('.page-tabs-content').animate({
                    marginLeft: 0 - scrollVal + 'px'
                }, "fast");
            }
        }
    }


    //左侧table点击添加新的iframe层
    function menuItem() {
        // 获取标识数据，获取ifrmae对象的url值；
        var dataUrl = $(this).attr('href'),
        //取出对应的data-index值；
            dataIndex = $(this).data('index'),
            //获取text值，并且取出前后的空格和制表符
            menuName = $.trim($(this).text()),
            flag = true;
        // 标签无值，或者url为空，结束程序并且返回
        if (dataUrl === undefined || $.trim(dataUrl).length === 0) return false;

        // 选项卡菜单已存在，遍历右侧table菜单栏
        $('.J_menuTab').each(function () {
            //是否已经存在对应的table菜单
            if ($(this).data('id') == dataUrl) {

                // 点击左侧菜单栏刷新当前页面
                if ($(this).hasClass('active')) {
                    $('.J_mainContent .J_iframe').each(function () {
                        // 还是以id值来进行判断，改成以数据驱动的方式应该会大量的减少代码的数量
                        if ($(this).data('id') == dataUrl) {
                            // $(this).html("");
                            $(this).attr('src', $(this).attr('src'));
                            return false;
                        }
                    });
                }

                if (!$(this).hasClass('active')) {
                    // 当前table菜单显示为被点击状态
                    $(this).addClass('active').siblings('.J_menuTab').removeClass('active');
                    // 滚动到当前选项卡，会使用自己的方式对其进行重写
                    scrollToTab(this);
                    // 显示tab对应的内容区
                    $('.J_mainContent .J_iframe').each(function () {
                        // 还是以id值来进行判断，改成以数据驱动的方式应该会大量的减少代码的数量
                        if ($(this).data('id') == dataUrl) {

                            //$(this)[0].contentWindow.document.body.innerText = "";

                            $(this).attr("src", $(this).attr('src'));
                            //$(this).children("body").html("");

                            //console.log($(this).children("body"));
                            $(this).show().siblings('.J_iframe').hide();
                            var maodian = window.location.href.split("#")[0];
                            window.location.href = maodian + "#" + dataUrl;
                            return false;
                        }
                    });
                }
                //选项卡函数将不执行
                flag = false;
                // 遍历函数将不执行
                return false;
            }
        });
        // 选项卡菜单不存在
        if (flag) {
            // 创建选项卡
            var str = '<a href="javascript:;" class="active J_menuTab" data-id="' + dataUrl + '">' + menuName + ' <i class="fa fa-times-circle"></i></a>';
            // 已知选项卡全部取消被选状态
            $('.J_menuTab').removeClass('active');

            // 添加选项卡对应的iframe
            var str1 = '<iframe class="J_iframe" name="iframe' + dataIndex + '" width="100%" height="100%" src="' + dataUrl + '" frameborder="0" data-id="' + dataUrl + '" seamless></iframe>';
            // 添加对应的iframe
            $('.J_mainContent').find('iframe.J_iframe').hide().parents('.J_mainContent').append(str1);


            // 添加选项卡
            $('.J_menuTabs .page-tabs-content').append(str);
            // 滚动到当前选项卡
            scrollToTab($('.J_menuTab.active'));
            var maodian = window.location.href.split("#")[0];
            window.location.href = maodian + "#" + dataUrl;
        }
        return false;
    }
    //左侧导航点击函数
    $('.J_menuItem').on('click', menuItem);

    if (window.location.href.split("#")[1]) {
        var dataUrl = window.location.href.split("#")[1];
        $(".J_menuItem").each(function () {
            if ($(this).attr('href') === dataUrl) {
                $(this).trigger("click");
            }
        });
    }
    // 关闭选项卡菜单
    function closeTab() {
        var closeTabId = $(this).parents('.J_menuTab').data('id');
        // 获取此元素的宽度
        var currentWidth = $(this).parents('.J_menuTab').width();

        // 当前元素处于活动状态
        if ($(this).parents('.J_menuTab').hasClass('active')) {

            // 当前元素后面有同辈元素，使后面的一个元素处于活动状态
            if ($(this).parents('.J_menuTab').next('.J_menuTab').size()) {
                // 获取后面紧接元素的id值
                var activeId = $(this).parents('.J_menuTab').next('.J_menuTab:eq(0)').data('id');
                // 后面元素显示为被点击状态
                $(this).parents('.J_menuTab').next('.J_menuTab:eq(0)').addClass('active');
                // 对应iframe显示，其余影藏
                $('.J_mainContent .J_iframe').each(function () {
                    if ($(this).data('id') == activeId) {
                        $(this).show().siblings('.J_iframe').hide();
                        var maodian = window.location.href.split("#")[0];
                        window.location.href = maodian + "#" + $(this).data("id");
                        return false;
                    }
                });

                var marginLeftVal = parseInt($('.page-tabs-content').css('margin-left'));
                //保持当前被选中的元素的位置不变
                if (marginLeftVal < 0) {
                    $('.page-tabs-content').animate({
                        marginLeft: (marginLeftVal + currentWidth) + 'px'
                    }, "fast");
                }

                //  移除当前选项卡
                $(this).parents('.J_menuTab').remove();

                // 移除tab对应的内容区
                $('.J_mainContent .J_iframe').each(function () {
                    if ($(this).data('id') == closeTabId) {
                        $(this).remove();
                        return false;
                    }
                });
            }

            // 当前元素后面没有同辈元素，使当前元素的上一个元素处于活动状态
            if ($(this).parents('.J_menuTab').prev('.J_menuTab').size()) {
                var activeIds = $(this).parents('.J_menuTab').prev('.J_menuTab:last').data('id');
                $(this).parents('.J_menuTab').prev('.J_menuTab:last').addClass('active');
                $('.J_mainContent .J_iframe').each(function () {
                    if ($(this).data('id') == activeIds) {
                        $(this).show().siblings('.J_iframe').hide();
                        var maodian = window.location.href.split("#")[0];
                        window.location.href = maodian + "#" + $(this).data("id");
                        return false;
                    }
                });

                //  移除当前选项卡
                $(this).parents('.J_menuTab').remove();

                // 移除tab对应的内容区
                $('.J_mainContent .J_iframe').each(function () {
                    if ($(this).data('id') == closeTabId) {
                        $(this).remove();
                        return false;
                    }
                });
            }
        }
            // 当前元素不处于活动状态
        else {
            //  移除当前选项卡
            $(this).parents('.J_menuTab').remove();

            // 移除相应tab对应的内容区
            $('.J_mainContent .J_iframe').each(function () {
                if ($(this).data('id') == closeTabId) {
                    $(this).remove();
                    return false;
                }
            });
            scrollToTab($('.J_menuTab.active'));
        }

        //当前选项卡缩进
        if ((calSumWidth($('.J_menuTab.active').prevAll()) + $('.J_menuTab.active').width()) <= Math.abs(parseInt($('.page-tabs-content').css('margin-left')))) {
            $('.page-tabs-content').animate({
                marginLeft: '0px'
            }, "fast");
        }
        return false;
    }
    $('.J_menuTabs').on('click', '.J_menuTab i', closeTab);
    //页面变化选项卡滚动函数
    (function () {
        var resizeReset = null;
        $(window).resize(function () {
            if ($(this).outerWidth() > 768) {
                clearTimeout(resizeReset);
                resizeReset = setTimeout(function () {
                    scrollToTab($('.J_menuTab.active'));
                }, 200);
            }
        });
    }());
    //关闭其他选项卡
    function closeOtherTabs() {
        $('.page-tabs-content').children("[data-id]").not(":first").not(".active").each(function () {
            $('.J_iframe[data-id="' + $(this).data('id') + '"]').remove();
            $(this).remove();
        });
        $('.page-tabs-content').css("margin-left", "0");
    }
    $('.J_tabCloseOther').on('click', closeOtherTabs);

    //滚动到已激活的选项卡
    function showActiveTab() {
        scrollToTab($('.J_menuTab.active'));
    }
    $('.J_tabShowActive').on('click', showActiveTab);

    // 点击选项卡菜单
    function activeTab() {
        if (!$(this).hasClass('active')) {
            var currentId = $(this).data('id');
            // 显示tab对应的内容区
            $('.J_mainContent .J_iframe').each(function () {
                if ($(this).data('id') == currentId) {
                    $(this).show().siblings('.J_iframe').hide();
                    var maodian = window.location.href.split("#")[0];
                    window.location.href = maodian + "#" + $(this).data("id");
                    return false;
                }
            });
            $(this).addClass('active').siblings('.J_menuTab').removeClass('active');
            scrollToTab(this);
        }
    }
    $('.J_menuTabs').on('click', '.J_menuTab', activeTab);
    //刷新iframe
    function refreshTab() {
        $('.J_mainContent .J_iframe').each(function () {
            // 还是以id值来进行判断，改成以数据驱动的方式应该会大量的减少代码的数量
            if ($(this).css("display") != "none") {
                $(this).attr('src', $(this).attr('src'));
            }
        });
    }

    $('#refresh').on('click', refreshTab);

    // 左移按扭
    $('.J_tabLeft').on('click', scrollTabLeft);

    // 右移按扭
    $('.J_tabRight').on('click', scrollTabRight);

    // 关闭全部
    $('.J_tabCloseAll').on('click', function () {
        $('.page-tabs-content').children("[data-id]").not(":first").each(function () {
            $('.J_iframe[data-id="' + $(this).data('id') + '"]').remove();
            $(this).remove();
        });
        $('.page-tabs-content').children("[data-id]:first").each(function () {
            $('.J_iframe[data-id="' + $(this).data('id') + '"]').show();
            var maodian = window.location.href.split("#")[0];
            window.location.href = maodian + "#" + $(this).data("id");
            $(this).addClass("active");
        });
        $('.page-tabs-content').css("margin-left", "0");
    });

});
