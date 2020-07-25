import React, { useState } from "react";
import { Paper, Tabs, Tab, MenuItem, Select, InputLabel, FormControl, FormHelperText, Grid, Container, Typography, CircularProgress } from "@material-ui/core";
import ChannelCard from "../channel/ChannelCard";
import Review from "../review/Review";
import axios from "axios";
import qs from 'qs';
import { backendDomain } from "../../App";

export default function SearchPage( { keyword } ) {
    const [value, setValue] = useState(0)
    const handleChange = (event, newValue) => {
        setValue(newValue);
    };
    const [sent, setSent] = useState(false);
    const [channels, setChannels] = useState([]);

    const [loading, setLoading] = useState(
        <Grid item xs={12} size="large" style={{display: "flex", justifyContent: "center"}}>
            <CircularProgress style={{
                width: "150px",
                height: "150px",
            }} color="secondary"/> 
        </Grid>
        );
    
    let term = qs.parse(keyword).keyword

    if (!sent) {
        let dbResult = [];
        axios.get(backendDomain + "/api/search?keyword=" + term + "&fullScan=false&scrape=false")
            .then(res => {
                dbResult = res.data;
                setChannels(res.data.splice(0, 27));
            });
        axios.get(backendDomain + "/api/search?keyword=" + term + "&fullScan=false&scrape=true")
            .then(res => {
                let result = res.data.concat(dbResult);
                result = result.slice(0, 27);
                setChannels(result);
                setLoading(null);
            }); 
        setSent(true);
    }

    let channelItems = channels.map(channel => (
            <Grid item>
                <ChannelCard channel={channel} />
            </Grid>
        )    
    )
    
    return (
        <Container style={{
            marginTop: "10px"
        }}>
            <Grid container spacing={3}>
                <Grid item xs={9}>
                    <Grid container spacing={2}>
                        <Grid item xs={12}>
                            <Paper style={{
                                display: "flex",
                                direction: "row",
                                alignItems: "center",
                                justifyContent: "space-around"
                            }}>
                                <Tabs
                                    indicatorColor="secondary"
                                    textColor="secondary"
                                    value={value}
                                    onChange={handleChange}
                                >
                                    <Tab label="Channels" />
                                    <Tab label="Reviews" />
                                </Tabs>
                                <FormControl >
                                    <Select
                                        value={0}
                                        onChange={handleChange}
                                    >
                                    <MenuItem value={0}> Name </MenuItem>
                                    <MenuItem value={1}> ID </MenuItem>
                                    <MenuItem value={2}> Youtube ID </MenuItem>
                                    </Select>
                                </FormControl>
                            </Paper>
                        </Grid>
                            {loading}
                            {channelItems}
                        </Grid>
                </Grid>
                <Grid item xs={3}> 
                        <Paper style={{height: "500px"}}>
                            <Typography> 
                                Ads
                            </Typography>
                        </Paper>
                    </Grid>
            </Grid>
        </Container>
    )
}