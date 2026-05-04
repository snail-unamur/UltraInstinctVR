from transformers import BertTokenizer, BertModel
import torch

model = BertModel.from_pretrained('bert-base-uncased')
tokenizer = BertTokenizer.from_pretrained('bert-base-uncased')

dummy = tokenizer("Hello world", return_tensors="pt")
torch.onnx.export(
    model,
    (dummy['input_ids'], dummy['attention_mask']),
    "bert.onnx",
    input_names=["input_ids", "attention_mask"],
    output_names=["output"],
    dynamic_axes={"input_ids": {1: "seq"}, "attention_mask": {1: "seq"}}
)
