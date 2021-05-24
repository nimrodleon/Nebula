package models

type Select2Item struct {
	ID   string `json:"id"`
	Text string `json:"text"`
}

type Select2 struct {
	Results []Select2Item `json:"results"`
}
