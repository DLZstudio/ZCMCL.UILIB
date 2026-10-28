// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

namespace ZCMCL.UILIB.UI.Animation.ValueProcessor;

public class NRotateTransformValueProcessor : IValueProcessor<NRotateTransform>
{
    public NRotateTransform Filter(NRotateTransform value) => value;
    
    public NRotateTransform Add(NRotateTransform value1, NRotateTransform value2) => value1 + value2;

    public NRotateTransform Subtract(NRotateTransform value1, NRotateTransform value2) => value1 - value2;
    
    public NRotateTransform Scale(NRotateTransform value, double factor) => value * (float)factor;
    
    public NRotateTransform DefaultValue() => new();
    
    public bool Equal(NRotateTransform value1, NRotateTransform value2) => value1 == value2;
}