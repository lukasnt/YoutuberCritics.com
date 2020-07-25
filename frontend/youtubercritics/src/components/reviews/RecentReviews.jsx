import React, { useState } from "react";
import axios from "axios";
import Reviews from "./Reviews";
import { backendDomain } from "../../App";

export default function RecentReviews( )
{
    const [sent, setSent] = useState(false);
    const [reviews, setReviews] = useState([]);

    if (!sent) {
        axios.get(backendDomain + "/api/reviews/recent")
            .then(res => setReviews(res.data));
        setSent(true);
    }

    return (
        <Reviews reviews={reviews} />
    );
}