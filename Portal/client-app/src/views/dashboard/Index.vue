<template>
    <AppPageContainer title="لوحة البيانات" subtitle="عرض الاحصائيات">
        <div class="card-area">
            
        </div>
    </AppPageContainer>
</template>

<script>
    // import LineChart from "@/components/charts/LineChart.js";
    // import BarChart from "@/components/charts/BarChart.js";
    // import PieChart from "@/components/charts/PieChart.js";

    export default {
        components: {
        //     LineChart,
        //     BarChart,
        //     PieChart
        },
        mounted() {
        },

        data() {
            return {
                colors: ["#3295a8", "#8231a8", "#193162", "#d54f09", "#c54c70", "#4e6e4c", "#0c0b6e", "#82f301", "#F055a8", "#E825a8"],
                byAge: {},
                byRegionsCenter: {},
                byHealthCenter: {},
                byPrimarySite: {},
                resultSet: {},
                series: {
                    labels: {
                        x: ["Orders.createdAt", "Orders.createdAt", "Orders.createdAt"],
                        y: ["Orders.createdAt", "Orders.createdAt", "Orders.createdAt"],
                    },
                    datasets: [
                        {
                            label: "المرضي المسجلين حسب سجلات المناطق",
                            backgroundColor: "rgba(135, 176, 94, 1)",
                            data: [1, 10, 20, 50, 40, 80],
                        },
                        {
                            label: "المرضي المسجلين حسب سجلات المناطق",
                            backgroundColor: "rgba(135, 176, 94, 1)",
                            data: [1, 10, 20, 50, 40, 80],
                        }
                    ]
                }
            };
        },
        methods: {
       
        },
    };
</script>
