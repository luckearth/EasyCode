import Vue from 'vue';
import { Component } from 'vue-property-decorator';

interface WeatherForecast {
    dateFormatted: string;
    temperatureC: number;
    temperatureF: number;
    summary: string;
}

@Component
export default class FetchDataComponent extends Vue {
    forecasts: WeatherForecast[] = [];

    mounted() {
        fetch('api/SampleData/WeatherForecasts',
            {
                method:"get",
                    headers: {
                        "Content-type": "application/json",
                        "Token": "f4c902c9ae5a2a9d8f84868ad064e706" }
                })
            .then(response => response.json() as Promise<WeatherForecast[]>)
            .then(data => {
                this.forecasts = data;
            });
    }
}
