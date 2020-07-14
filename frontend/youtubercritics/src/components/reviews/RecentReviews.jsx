import React, { useState } from "react";
import axios from "axios";
import Reviews from "./Reviews";

export default function RecentReviews( )
{
    const [sent, setSent] = useState(false);
    const [reviews, setReviews] = useState([]);

    if (!sent) {
        axios.get("https://localhost:5001/api/reviews/recent")
            .then(res => setReviews(res.data));
        setSent(true);
    }

    return (
        <Reviews reviews={reviews} />
    );
}